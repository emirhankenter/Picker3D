using System;
using System.Collections;
using System.Collections.Generic;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class InputController : MonoBehaviour, IDisposable
    {
        public static event Action<Vector2> PressPerformed; 
        public static event Action<Vector2> PressCanceled;
        public static event Action<Vector2> MovePerformed;

        private static bool _isPressing;

        public static Vector3 MousePosition{ get; private set; }
        public static bool IsActive { get; private set; }

        private const float _updateIteration = 0.001f;

        private string _mouseEventRoutineKey => $"MouseEventRoutine{GetInstanceID()}";
        public void Init()
        {
            MouseEventRoutine().StartCoroutine(_mouseEventRoutineKey);
        }

        public void Dispose()
        {
            if (CoroutineController.IsCoroutineRunning(_mouseEventRoutineKey))
            {
                CoroutineController.StopCoroutine(_mouseEventRoutineKey);
            }
        }

        public static void Toggle(bool state)
        {
            IsActive = state;
            _isPressing = false;
        }

        private IEnumerator MouseEventRoutine()
        {
            while (true)
            {
                if (!IsActive) yield return new WaitForSeconds(_updateIteration);

                MousePosition = Input.mousePosition;

                if (_isPressing)
                {
                    MovePerformed?.Invoke(MousePosition);
                }

                if (Input.GetMouseButtonDown(0) && !_isPressing)
                {
                    _isPressing = true;
                    PressPerformed?.Invoke(MousePosition);
                }

                if (Input.GetMouseButtonUp(0) && _isPressing)
                {
                    _isPressing = false;
                    PressCanceled?.Invoke(MousePosition);
                }
                yield return new WaitForSeconds(_updateIteration);
            }
        }
    }
}
