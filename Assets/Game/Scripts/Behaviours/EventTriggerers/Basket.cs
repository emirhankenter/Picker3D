using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Scripts.Controllers;
using MekCoroutine;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class Basket : EventTriggerer
    {
        public event Action<bool> Result;

        [SerializeField] private TextMeshPro _targetText;
        [SerializeField] private MeshRenderer _ground;

        [SerializeField] private int _targetCount;
        private int _current;

        private List<ICollectible> _collectibles = new List<ICollectible>();

        private string _timeOutRoutineKey => $"timeOutRoutine{GetInstanceID()}";

        public void Init()
        {
            _targetText.text = $"{_current}/{_targetCount}";
        }

        public void Activate(float timer = 2f)
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

            _targetText.text = $"{_current}/{_targetCount}";
        }

        protected override void TriggerExit(ICollectible collectible)
        {
            _collectibles.Remove(collectible);
            _current--;

            _targetText.text = $"{_current}/{_targetCount}";
        }

        protected IEnumerator TimeOutRoutine(float timer)
        {
            while (timer >= 0f)
            {
                timer -= Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
            var canPass = _current >= _targetCount;

            foreach (var collectible in _collectibles)
            {
                collectible.Collected();
            }

            yield return new WaitForSeconds(0.5f);
            if (canPass) AllowPass();
            Result?.Invoke(canPass);
        }

        private void AllowPass()
        {
            _ground.transform.DOLocalMove(new Vector3(_ground.transform.localPosition.x, 0f, _ground.transform.localPosition.z), 0.4f).SetEase(Ease.OutBack);

            _ground.material.DOColor(new Color(91f / 255f, 137 / 255f, 14f / 255f, 1), 0.4f).SetEase(Ease.Linear);
        }
    }
}
