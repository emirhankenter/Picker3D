using System;
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

        [SerializeField] private List<StageBehaviour> _stagesssssss;
        //[SerializeField] private List<StageFinishTriggerer> _stages;
        //[SerializeField] private List<Basket> _baskets;

        public PlayerController Player => _playerController;

        private int _currentStageIndex;

        public void Initialize()
        {
            _currentStageIndex = 0;

            _playerController.Init();
            _playerController.OnStageCompleted += OnPlayerPassedStage;

            foreach (var stage in _stagesssssss)
            {
                stage.Init();
            }

            Started?.Invoke();
        }

        public void Dispose()
        {
            _playerController.Dispose();
            _playerController.OnStageCompleted -= OnPlayerPassedStage;

            foreach (var stage in _stagesssssss)
            {
                stage.Dispose();
            }
        }

        private void OnPlayerPassedStage(Action callback)
        {
            _stagesssssss[_currentStageIndex].Result += OnStageResultReturned;
            _stagesssssss[_currentStageIndex].StartEvaluation();

            _continuePlayer = callback;
        }

        private void OnStageResultReturned(bool state)
        {
            _stagesssssss[_currentStageIndex].Result -= OnStageResultReturned;

            //_stagesssssss.Remove(_stagesssssss.First());

            if (state)
            {
                Debug.Log($"StageCompleted: {_stagesssssss[_currentStageIndex].name}");
                if (_currentStageIndex >= _stagesssssss.Count - 1)
                {
                    Debug.Log("LevelFinished");
                    OnPlayerFinished(true);
                }
                else if(_currentStageIndex == _stagesssssss.Count - 2)
                {
                    _playerController.OnEnteredFinalStage();
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
