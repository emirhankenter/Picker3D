using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Controllers;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class Basket : EventTriggerer
    {
        public event Action<bool> Result;

        [SerializeField] private Transform _ground;

        [SerializeField] private int _targetCount;
        private int _current;

        private List<ICollectible> _collectibles = new List<ICollectible>();

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

            _collectibles.Clear();
        }

        protected override void TriggerEnter(ICollectible collectible)
        {
            _collectibles.Add(collectible);
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
            var canPass = _current >= _targetCount;

            foreach (var collectible in _collectibles)
            {
                collectible.Collected();
            }

            if(canPass) AllowPass();
            Result?.Invoke(canPass);
        }

        private void AllowPass()
        {
            _ground.DOLocalMove(new Vector3(_ground.localPosition.x, 0f, _ground.localPosition.z), 0.3f).SetEase(Ease.Linear);
        }
    }
}
