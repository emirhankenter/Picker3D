using System;
using Game.Scripts.Behaviours.EventTriggerers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class StageBehaviour : MonoBehaviour
    {
        public event Action<bool> Result;

        [SerializeField] private StageFinishTriggerer _stageFinishTriggerer;
        [SerializeField] private Basket _basket;

        public void Init()
        {
            _basket.Result += OnBasketResult;

            _basket.Init();
        }

        public void StartEvaluation()
        {
            _basket.Activate();
        }

        public void Dispose()
        {
            _basket.Dispose();
        }

        private void OnBasketResult(bool result)
        {
            _basket.Result -= OnBasketResult;
            Result?.Invoke(result);
        }
    }
}
