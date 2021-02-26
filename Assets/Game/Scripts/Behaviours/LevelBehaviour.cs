using System;
using System.Collections.Generic;
using Game.Scripts.Behaviours.EventTriggerers;
using Game.Scripts.Controllers;
using MekCoroutine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public static event Action Started;
        public Action<bool> Completed;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private List<StageFinishTriggerer> _stages;

        public void Initialize()
        {
            _playerController.Init();
            _playerController.OnStageCompleted += OnPlayerPassedStage;

            Started?.Invoke();
        }

        public void Dispose()
        {
            _playerController.Dispose();
            _playerController.OnStageCompleted -= OnPlayerPassedStage;
        }

        private void OnPlayerPassedStage(StageFinishTriggerer stage, Action callback)
        {
            _stages.Remove(stage);

            if (_stages.Count == 0)
            {
                OnPlayerFinished(true);
            }
            else
            {
                CoroutineController.DoAfterGivenTime(2f, callback);
            }
        }

        private void OnPlayerFinished(bool isSuccess)
        {
            Completed?.Invoke(isSuccess);
        }
    }
}
