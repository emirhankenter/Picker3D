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
        public static event Action StageCompleted;
        public static event Action Started;
        public Action<bool> Completed;

        private event Action _continuePlayer;

        private PlayerController _playerController;

        [SerializeField] private List<StageBehaviour> _stagesssssss;

        public PlayerController Player => _playerController;
        public int StageCount => _stagesssssss.Count;

        private int _currentStageIndex;
        public int CurrentStageIndex => _currentStageIndex;

        public int Multiplier { get; private set; }

        public void Initialize(PlayerController player)
        {
            _currentStageIndex = 0;
            _playerController = player;

            _playerController.OnStageCompleted += OnPlayerPassedStage;

            foreach (var stage in _stagesssssss)
            {
                stage.Init();
            }

            Started?.Invoke();
        }

        public void Dispose()
        {
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
            StageCompleted?.Invoke();

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
                    FinalStage.Multiplied += OnMultipliedWith;
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
            _currentStageIndex++;
        }

        private void OnMultipliedWith(int multiplier)
        {
            FinalStage.Multiplied -= OnMultipliedWith;
            Multiplier = multiplier;
        }

        private void OnPlayerFinished(bool isSuccess)
        {
            Completed?.Invoke(isSuccess);
        }
    }
}
