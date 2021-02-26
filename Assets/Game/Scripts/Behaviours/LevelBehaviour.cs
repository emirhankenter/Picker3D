using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public static event Action Started;
        public Action<bool> Completed;

        public void Initialize()
        {
            Started?.Invoke();
        }

        public void Dispose()
        {
        }

        [Button]
        private void OnPlayerFinished(bool isSuccess)
        {
            Completed?.Invoke(isSuccess);
        }
    }
}
