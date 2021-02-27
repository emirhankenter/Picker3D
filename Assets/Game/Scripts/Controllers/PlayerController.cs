using System;
using System.Collections;
using Game.Scripts.Behaviours;
using Game.Scripts.View.Elements;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public interface IPicker
    {
        void OnStageCleared();
        void OnPassedFinishLine();
    }
    public interface ICollectible
    {
        void Collected();
        void PushForward();
        void Bounce();
    }

    public class PlayerController : MonoBehaviour, IPicker
    {
        public event Action<Action> OnStageCompleted;

        [SerializeField] private PickerStorage _storage;

        private const float _steerSpeed = 12f;
        private const float _maxXVelocity = 36f;
        private const float _forwardSpeed = 15f;
        private const float _bounds = 7.5f;
        private const float _progressIncrementPerClick = 0.12f;
        private const float _progressDecrementPerSecond = 0.3f;

        private float _progress; //between 0 and 1
        private const float _speedMultiplier = 12f;
        private Rigidbody _rb;
        private Vector2 _firstInput;
        private Vector2 _drag;
        public float Speed => _rb.velocity.magnitude;

        private string _movementRoutineKey => $"movementRoutine{GetInstanceID()}";
        private string _tapRoutineKey => $"tapRoutine{GetInstanceID()}";

        private bool _inTapTapZone;
        private bool _isFirstInput = true;

        public void Init()
        {
            _rb = GetComponent<Rigidbody>();
            _isFirstInput = true;
            RegisterEvents();
        }

        public void Dispose()
        {
            UnRegisterEvents();

            ToggleMovement(false);
            _rb.velocity = Vector3.zero;

            CoroutineController.ToggleRoutine(false, _tapRoutineKey, TapTapRoutine());
        }

        private void RegisterEvents()
        {
            InputController.PressPerformed += OnPressPerformed;
            InputController.PressCanceled += OnPressCanceled;
        }

        private void UnRegisterEvents()
        {
            InputController.PressPerformed -= OnPressPerformed;
            InputController.PressCanceled -= OnPressCanceled;
        }

        private void ToggleMovement(bool state)
        {
            CoroutineController.ToggleRoutine(state, _movementRoutineKey, MovementRoutine());
        }

        private void OnPressPerformed(Vector2 obj)
        {
            _firstInput = obj;

            if (_isFirstInput)
            {
                _isFirstInput = false;
                ToggleMovement(true);
            }

            InputController.MovePerformed += OnMovePerformed;

            if (_inTapTapZone || CoroutineController.IsCoroutineRunning(_tapRoutineKey))
            {
                _progress = Mathf.Clamp(_progress + _progressIncrementPerClick, 0f, 1f);
            }
        }

        private void OnPressCanceled(Vector2 obj)
        {
            _drag = Vector2.zero;
            _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
            InputController.MovePerformed -= OnMovePerformed;
        }

        private void OnMovePerformed(Vector2 obj)
        {
            _drag = obj - _firstInput;

            _firstInput = obj;
        }

        private IEnumerator MovementRoutine()
        {
            while (true)
            { 
                var velocity = Vector3.Lerp(_rb.velocity, new Vector3(_drag.x * _steerSpeed, 0.6f, _forwardSpeed), Time.fixedDeltaTime* 15f);

                velocity.x = Mathf.Clamp(velocity.x, -_maxXVelocity, _maxXVelocity);

                _rb.velocity = velocity;

                _rb.transform.position = new Vector3(Mathf.Clamp(_rb.transform.position.x, -_bounds, _bounds), _rb.transform.position.y,
                    _rb.transform.position.z);

                yield return new WaitForFixedUpdate();
            }
        }

        private IEnumerator TapTapRoutine()
        {
            while (true)
            {
                //_rb.AddForce(transform.forward * 1f);

                _progress = Mathf.Lerp(_progress, Mathf.Clamp(_progress - _progressIncrementPerClick, 0f, 1f), Time.fixedDeltaTime);

                var velocity = Vector3.Lerp(_rb.velocity, new Vector3(0, 0.4f, _forwardSpeed), Time.fixedDeltaTime * 15f);

                velocity.z += _progress * _speedMultiplier;

                _rb.velocity = velocity;

                VerticalProgressBar.UpdateValue(_progress);

                _rb.transform.position = new Vector3(Mathf.Clamp(_rb.transform.position.x, -_bounds, _bounds), _rb.transform.position.y,
                    _rb.transform.position.z);
                Debug.Log($"Progress: {_progress}");
                yield return new WaitForFixedUpdate();
            }
        }

        public void OnStageCleared()
        {
            ToggleMovement(false);
            _rb.velocity = Vector3.zero;
            _storage.PushCollectibles();

            OnStageCompleted?.Invoke(() => ToggleMovement(true));
        }

        public void OnEnteredFinalStage()
        {
            Debug.Log("TapTapAreaEntered");
            _inTapTapZone = true;
            VerticalProgressBar.Activate();
            CoroutineController.ToggleRoutine(true, _tapRoutineKey, TapTapRoutine());
        }

        public void OnPassedFinishLine()
        {
            _inTapTapZone = false;
            CoroutineController.ToggleRoutine(false, _tapRoutineKey, TapTapRoutine());
            VerticalProgressBar.Stop();
            OnStageCompleted?.Invoke(() =>
            {});
            //ToggleMovement(false);
            Debug.Log("PassedFinishLine");
        }
    }
}
