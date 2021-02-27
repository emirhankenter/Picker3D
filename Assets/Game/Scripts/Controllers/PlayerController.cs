using System;
using System.Collections;
using Game.Scripts.Behaviours.EventTriggerers;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public interface IPicker
    {
        void OnStageCleared(StageFinishTriggerer stage);
    }
    public interface ICollectible
    {
        void Collected();
    }

    public class PlayerController : MonoBehaviour, IPicker
    {
        public event Action<StageFinishTriggerer, Action> OnStageCompleted;

        private Vector2 _firstInput;
        private Vector2 _drag;

        private const float _steerSpeed = 12f;
        private const float _maxXVelocity = 36f;
        private const float _forwardSpeed = 15f;
        private const float _bounds = 7.5f;

        private Rigidbody _rb;

        private string _movementRoutineKey => $"movementRoutine{GetInstanceID()}";

        //private void Awake()
        //{
        //    Init();
        //}

        //private void OnDestroy()
        //{
        //    Dispose();
        //}

        public void Init()
        {
            _rb = GetComponent<Rigidbody>();

            RegisterEvents();

            ToggleMovement(true);
        }

        public void Dispose()
        {
            UnRegisterEvents();

            ToggleMovement(false);
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
            _rb.velocity = Vector3.zero;
            CoroutineController.ToggleRoutine(state, _movementRoutineKey, MovementRoutine());
        }

        private void OnPressPerformed(Vector2 obj)
        {
            _firstInput = obj;

            InputController.MovePerformed += OnMovePerformed;
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
                var velocity = Vector3.Lerp(_rb.velocity, new Vector3(_drag.x * _steerSpeed, 0.4f, _forwardSpeed), Time.fixedDeltaTime* 15f);

                velocity.x = Mathf.Clamp(velocity.x, -_maxXVelocity, _maxXVelocity);

                _rb.velocity = velocity;

                _rb.transform.position = new Vector3(Mathf.Clamp(_rb.transform.position.x, -_bounds, _bounds), _rb.transform.position.y,
                    _rb.transform.position.z);

                Debug.Log($"XVelocity: {_rb.velocity.x}");

                yield return new WaitForFixedUpdate();
            }
        }

        public void OnStageCleared(StageFinishTriggerer stage)
        {
            Debug.Log($"Stage has cleared!");

            ToggleMovement(false);

            OnStageCompleted?.Invoke(stage, () => ToggleMovement(true));
        }
    }
}
