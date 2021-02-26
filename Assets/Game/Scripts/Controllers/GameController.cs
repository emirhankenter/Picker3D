using System;
using System.Collections.Generic;
using Assets.Game.Scripts.Behaviours;
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
            //ViewController.Instance.MainMenuView.Close();
            //ViewController.Instance.InGameView.Open(new InGameViewParameters());

            //_player.Initialize();
        }

        private void DisposeLevel()
        {
            CurrentLevel.Dispose();

            Destroy(CurrentLevel.gameObject);

            PrepareLevel();
        }

        private void NextLevel()
        {
            DisposeLevel();
        }

        [Button]
        private void OnLevelCompleted(bool isSuccess)
        {
            CurrentLevel.Completed -= OnLevelCompleted;

            Navigation.Panel.Change(ViewTypes.GameOverPanel);//todo delete
            if (isSuccess)
            {
                var earnAmount = PlayerData.Instance.PlayerLevel * 50;
                PlayConfetti?.Invoke(() =>
                {
                    Navigation.Panel.Change(ViewTypes.GameOverPanel);
                    //ViewController.Instance.InGameView.Close();
                    //ViewController.Instance.GameOverView.Open(new GameOverViewParameters(earnAmount, OnRewardClaimed));
                });
                PlayerData.Instance.PlayerLevel++;
                //PlayerData.Coin += earnAmount;
            }
            else
            {
                //ViewController.Instance.InGameView.Close();
                DisposeLevel();
            }
        }

        private void OnRewardClaimed()
        {
            CoroutineController.DoAfterGivenTime(2f, () =>
            {
                //ViewController.Instance.GameOverView.Close();
                NextLevel();
            });
        }

    }
}
