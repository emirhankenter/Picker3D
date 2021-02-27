using System;
using System.Collections;
using Game.Scripts.Controllers;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class Basket : EventTriggerer
    {
        public event Action<bool> Result; 

        [SerializeField] private int _targetCount;
        private int _current;

        private string _timeOutRoutineKey => $"timeOutRoutine{GetInstanceID()}";

        public void Init(float timer = 2f)
        {
            if (!CoroutineController.IsCoroutineRunning(_timeOutRoutineKey))
            {
                CoroutineController.StartCoroutine(_timeOutRoutineKey, TimeOutRoutine(timer));
            }
        }

        public void Dispose()
        {
            if (CoroutineController.IsCoroutineRunning(_timeOutRoutineKey))
            {
                CoroutineController.StopCoroutine(_timeOutRoutineKey);
            }
        }

        protected override void TriggerEnter(ICollectible collectible)
        {
            _current++;
        }

        protected IEnumerator TimeOutRoutine(float timer)
        {
            while (timer >= 0f)
            {
                timer -= Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
            Debug.Log($"Timeout with: {_current >= _targetCount}, {_current}/{_targetCount}");
            Result?.Invoke(_current >= _targetCount);
        }
    }
}
