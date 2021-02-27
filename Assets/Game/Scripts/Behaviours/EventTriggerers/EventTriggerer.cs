using System;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours.EventTriggerers
{
    public abstract class EventTriggerer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                TriggerEnter(picker);
            }


            if (other.TryGetComponent(out ICollectible collectible))
            {
                TriggerEnter(collectible);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                TriggerExit(collectible);
            }
        }

        protected virtual void TriggerEnter(IPicker picker)
        {
        }
        protected virtual void TriggerEnter(ICollectible collectible)
        {
        }
        protected virtual void TriggerExit(ICollectible collectible)
        {
        }

        private void OnDrawGizmos()
        {
            DrawGizmos();
        }

        protected virtual void DrawGizmos(){}
    }
}
