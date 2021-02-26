using System;
using System.Collections.Generic;
using Game.Scripts.Behaviours;
using Game.Scripts.Models;
using Mek.Controllers;
using Mek.Localization;
using MekCoroutine;
using MekNavigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : GameBaseController
    {
        public static event Action<Action> PlayConfetti;

        //[SerializeField] private PlayerBehaviour _player;
        //[SerializeField] private ParticleSystem _confetti;

        [SerializeField] private List<LevelBehaviour> _levels;
        public LevelBehaviour CurrentLevel { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            LocalizationManager.Init();
            LocalizationManager.SetLanguage(Language.Turkish);
            PrepareLevel();
        }


        private void PrepareLevel()
        {
            CurrentLevel = Instantiate(_levels[(PlayerData.Instance.PlayerLevel - 1) % _levels.Count]);
            CurrentLevel.Initialize();

            CurrentLevel.Completed += OnLevelCompleted;

            Navigation.Panel.Change(ViewTypes.MainMenuPanel);

            //ViewController.Instance.LoadingView.Close();
            //ViewController.Instance.MainMenuView.Open(new MainMenuViewParameters(StartGame));
        }

        [Button]
        private void StartGame()
        {
            Navigation.Panel.Change(ViewTypes.InGamePanel);

            //_player.Initialize();
        }

        private void DisposeLevel()
        {
            CurrentLevel.Dispose();

            Destroy(CurrentLevel.gameObject);

            PrepareLevel();
        }

        [Button]
        private void OnLevelCompleted(bool isSuccess)
        {
            CurrentLevel.Completed -= OnLevelCompleted;

            var earnAmount = PlayerData.Instance.PlayerLevel * 50;

            Navigation.Panel.Change(new GameOverViewParams(isSuccess, earnAmount, OnRewardClaimed));
            if (isSuccess)
            {
                PlayConfetti?.Invoke(() =>
                {
                    Navigation.Panel.Change(new GameOverViewParams(true, earnAmount, OnRewardClaimed));
                });
                PlayerData.Instance.PlayerLevel++;
                PlayerData.Instance.Coin += earnAmount;
            }
            else
            {
                Navigation.Panel.Change(new GameOverViewParams(false, earnAmount, OnRewardClaimed));
                //DisposeLevel();
            }
        }

        private void OnRewardClaimed()
        {
            CoroutineController.DoAfterGivenTime(2f, () =>
            {
                //ViewController.Instance.GameOverView.Close();
                DisposeLevel();
            });
        }

    }
}
