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
        }

        protected virtual void TriggerEnter(IPicker picker)
        {
        }
    }
}
