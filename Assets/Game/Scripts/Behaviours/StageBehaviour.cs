using Game.Scripts.Behaviours.EventTriggerers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class StageBehaviour : MonoBehaviour
    {
        [SerializeField] private StageFinishTriggerer _stageFinishTriggerer;
        [SerializeField] private Basket _basket;

        public StageFinishTriggerer StageFinishTriggerer => _stageFinishTriggerer;
        public Basket Basket => _basket;
    }
}
