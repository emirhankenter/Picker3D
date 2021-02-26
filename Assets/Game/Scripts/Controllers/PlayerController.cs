﻿using System;
using System.Collections;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public interface IPicker
    {
        void OnStageCleared();
    }

    public class PlayerController : MonoBehaviour, IPicker
    {
        private Vector2 _firstInput;
        private Vector2 _drag;

        private const float _steerSpeed = 25f;
        private const float _forwardSpeed = 15f;
        private const float _bounds = 7.5f;

        private Rigidbody _rb;

        private string _movementRoutineKey => $"movementRoutine{GetInstanceID()}";

        private void Awake()
        {
            Init();
        }

        private void OnDestroy()
        {
            Dispose();
        }

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
                //var pos = Vector3.Lerp(transform.position, transform.position + transform.forward * _forwardSpeed+ transform.right * _drag.x, Time.fixedDeltaTime);

                //pos.x = Mathf.Clamp(pos.x, -_bounds, _bounds);
                //_rb.MovePosition(pos);

                _rb.velocity = Vector3.Lerp(_rb.velocity, new Vector3(_drag.normalized.x * _steerSpeed, 0, _forwardSpeed), Time.fixedDeltaTime* 15f);
                _rb.transform.position = new Vector3(Mathf.Clamp(_rb.transform.position.x, -_bounds, _bounds), 0,
                    _rb.transform.position.z);
                //transform.position = pos;

                yield return new WaitForFixedUpdate();
            }
        }

        public void OnStageCleared()
        {
            Debug.Log($"Stage has cleared!");

            ToggleMovement(false);

            CoroutineController.DoAfterGivenTime(2f, () =>
            {
                ToggleMovement(true);
            });
        }
    }
}