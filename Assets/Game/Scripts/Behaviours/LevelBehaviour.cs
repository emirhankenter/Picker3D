﻿using System;
using System.Collections.Generic;
using Game.Scripts.Behaviours.EventTriggerers;
using Game.Scripts.Controllers;
using Mek.Extensions;
using MekCoroutine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class LevelBehaviour : MonoBehaviour
    {
        public static event Action Started;
        public Action<bool> Completed;

        private event Action _continuePlayer;

        [SerializeField] private PlayerController _playerController;

        [SerializeField] private List<StageFinishTriggerer> _stages;
        [SerializeField] private List<Basket> _baskets;

        private int _currentStageIndex;

        public void Initialize()
        {
            _currentStageIndex = 0;

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
            _baskets[_currentStageIndex].Result += OnBasketResultReturned;
            _baskets[_currentStageIndex].Init();

            _continuePlayer = callback;
        }

        private void OnBasketResultReturned(bool state)
        {
            _baskets[_currentStageIndex].Result -= OnBasketResultReturned;

            _stages.Remove(_stages.First());

            if (state)
            {
                Debug.Log("BasketTargetReached");
                if (_stages.Count == 0)
                {
                    Debug.Log("LevelFinished");
                    OnPlayerFinished(true);
                }
                else
                {
                    Debug.Log("Continue");
                    CoroutineController.DoAfterGivenTime(0.2f, _continuePlayer);
                }
            }
            else
            {
                OnPlayerFinished(false);
            }
            //_baskets[_currentStageIndex].Dispose();
            _currentStageIndex++;
        }

        private void OnPlayerFinished(bool isSuccess)
        {
            Completed?.Invoke(isSuccess);
        }
    }
}
