using Game.Scripts.Behaviours.EventTriggerers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class CollectionStage : StageBehaviour
    {
        [SerializeField] private int _target = 3;
        [SerializeField] private Basket _basket;
        [SerializeField] private GateBehaviour _gate;

        public override void Init()
        {
            base.Init();
            _basket.Result += OnBasketResult;

            _basket.Init(_target);
        }

        public override void StartEvaluation()
        {
            base.StartEvaluation();
            _basket.Activate();
        }

        public override void Dispose()
        {
            base.Dispose();
            _basket.Dispose();
        }

        private void OnBasketResult(bool result)
        {
            _basket.Result -= OnBasketResult;
            if (result)
            {
                _gate.OpenSesame(() =>
                {
                    Complete(true);
                    //Result?.Invoke(true);
                });
            }
            else
            {
                Complete(false);
                //Result?.Invoke(false);
            }
        }
    }
}
