using System;
using UnityEngine;

namespace Assets.Game.Scripts.Behaviours
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

        private void OnPlayerFinished(bool isSuccess)
        {
            Completed?.Invoke(isSuccess);
        }
    }
}
