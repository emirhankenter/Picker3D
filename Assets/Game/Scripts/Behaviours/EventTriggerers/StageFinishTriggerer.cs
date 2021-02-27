using System;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public class StageFinishTriggerer : EventTriggerer
    {
        [SerializeField] private Collider _collider;

        private Color _gizmosColor = new Color(1,0,1, 0.5f);

        protected override void TriggerEnter(IPicker picker)
        {
            picker.OnStageCleared();
        }

        private void OnDrawGizmosSelected()
        {
            if (_collider)
            {
                Gizmos.color = _gizmosColor;
                Gizmos.DrawCube(_collider.bounds.center, _collider.bounds.size);
            }
        }
    }
}
