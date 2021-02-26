using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MekCoroutine
{
    public class CoroutineWorker : MonoBehaviour
    {
        private static CoroutineWorker _instance;
        public static CoroutineWorker Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("CoroutineWorker");
                    _instance = go.AddComponent<CoroutineWorker>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }
    }
}