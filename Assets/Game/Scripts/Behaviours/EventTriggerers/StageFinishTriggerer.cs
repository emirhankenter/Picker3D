using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class StageFinishTriggerer : EventTriggerer
    {
        protected override void TriggerEnter(IPicker picker)
        {
            picker.OnStageCleared();
        }
    }
}
