using System;
using System.Collections.Generic;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class PickerStorage : MonoBehaviour
    {
        private List<ICollectible> _collectibles = new List<ICollectible>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                if (!_collectibles.Contains(collectible))
                {
                    _collectibles.Add(collectible);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                if (_collectibles.Contains(collectible))
                {
                    _collectibles.Remove(collectible);
                }
            }
        }

        public void PushCollectibles()
        {
            foreach (var collectible in _collectibles)
            {
                collectible.PushForward();
            }
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out ICollectible collectible))
            {
                collectible.Bounce();
            }
        }
    }
}
