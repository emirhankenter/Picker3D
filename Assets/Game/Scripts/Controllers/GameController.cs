using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game.Scripts.Behaviours;
using Game.Scripts.Models;
using Game.Scripts.Models.ViewParams;
using Game.Scripts.Utilities;
using Mek.Controllers;
using Mek.Extensions;
using Mek.Localization;
using MekCoroutine;
using MekNavigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class GameController : GameBaseController
    {
        //public static event Action<Action> PlayConfetti;

        //[SerializeField] private ParticleSystem _confetti;

        [SerializeField] private PlayerController _player;
        [SerializeField] private List<LevelBehaviour> _levels;

        //[SerializeField] private InputController _inputController;
        [SerializeField] private DragHandler _dragHandler;
        public LevelBehaviour CurrentLevel { get; private set; }
        public LevelBehaviour NextLevel { get; private set; }

        public DragHandler DragHandler => _dragHandler;

        protected override void Awake()
        {
            base.Awake();

            LocalizationManager.Init();
            LocalizationManager.SetLanguage((Language)PlayerData.Instance.Language);
            PrepareLevel();
        }


        private void PrepareLevel()
        {
            _completedWithSucces = false;
            if (NextLevel)
            {
                CurrentLevel = NextLevel;
            }
            else
            {
                LevelBehaviour level;
                if (PlayerData.Instance.PlayerLevel > _levels.Count)
                {
                    level = _levels.RandomElement();
                    while (int.Parse(level.name.Replace("Level_", "")) <= 2)
                    {
                        level = _levels.RandomElement();
                    }
                }
                else
                {
                    level = _levels[(PlayerData.Instance.PlayerLevel - 1) % _levels.Count];
                }
                //level = PlayerData.Instance.PlayerLevel > _levels.Count
                //    ? _levels.RandomElement()
                //    : _levels[(PlayerData.Instance.PlayerLevel - 1) % _levels.Count];

                CurrentLevel = Instantiate(level);
            }
            CurrentLevel.Initialize(_player);
            PrepareNextLevel();

            CurrentLevel.Completed += OnLevelCompleted;

            Navigation.Panel.Change(ViewTypes.MainMenuPanel);

            CoroutineController.DoAfterFixedUpdate(() =>
            {
                _player.Init();
            });

            //CrossfadeTransition.FadeOut(1f, () =>
            //{
            //    DragHandler.ToggleInput(true);
            //    //InputController.Toggle(true);
            //});

            CoroutineController.DoAfterGivenTime(1f, () =>
            {
                DragHandler.ToggleInput(true);
            });
        }

        private void DisposeLevel()
        {
            CurrentLevel.Dispose();
            _player.Dispose();
            if (_completedWithSucces)
            {
                var difference = CurrentLevel.StageCount * 100f + 60f;

                CurrentLevel.transform.position = new Vector3(0, 0, CurrentLevel.transform.position.z - difference);
                NextLevel.transform.position = new Vector3(0, 0, NextLevel.transform.position.z - difference);
                _player.transform.position = new Vector3(0, 0, _player.transform.position.z - difference);
                _player.transform.DOMove(new Vector3(0, 0.01f, 0), 1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    PrepareLevel();
                });
                var currentLevelForDelayedDestroy = CurrentLevel.gameObject;
                Destroy(currentLevelForDelayedDestroy, 3);
                Navigation.Panel.CloseActiveContent();
            }
            else
            {
                Destroy(CurrentLevel.gameObject);
                Destroy(NextLevel.gameObject);

                CurrentLevel = null;
                NextLevel = null;
                _player.transform.position = new Vector3(0, 0.01f, 0);

                PrepareLevel();
            }
        }

        private bool _completedWithSucces;
        [Button]
        private void OnLevelCompleted(bool isSuccess)
        {
            _completedWithSucces = isSuccess;
            CurrentLevel.Completed -= OnLevelCompleted;

            var earnAmount = PlayerData.Instance.PlayerLevel * 50;

            DragHandler.ToggleInput(false);
            Debug.Log("LevelCompleted");
            Navigation.Panel.Change(new GameOverViewParams(isSuccess, earnAmount * (isSuccess ? 1 : 0), OnRewardClaimed));
            if (isSuccess)
            {
                //PlayConfetti?.Invoke(() =>
                //{
                //    Navigation.Panel.Change(new GameOverViewParams(true, earnAmount, OnRewardClaimed));
                //});
                PlayerData.Instance.PlayerLevel++;
                PlayerData.Instance.Coin += earnAmount;
            }
            else
            {
                //Navigation.Panel.Change(new GameOverViewParams(false, earnAmount, OnRewardClaimed));
            }
        }

        private void OnRewardClaimed()
        {
            CoroutineController.DoAfterGivenTime(1f, () =>
            {
                DisposeLevel();
            });
        }

        private void PrepareNextLevel()
        {
            LevelBehaviour level;
            if (PlayerData.Instance.PlayerLevel + 1 > _levels.Count)
            {
                level = _levels.RandomElement();
                while (level.name == CurrentLevel.name.Replace("(Clone)", "") || int.Parse(level.name.Replace("Level_", "")) <= 2)
                {
                    level = _levels.RandomElement();
                }
            }
            else
            {
                level = _levels[(PlayerData.Instance.PlayerLevel) % _levels.Count];
            }
            //level = PlayerData.Instance.PlayerLevel + 1 > _levels.Count
            //    ? _levels.RandomElement()
            //    : _levels[(PlayerData.Instance.PlayerLevel) % _levels.Count];

            NextLevel = Instantiate(level);
            NextLevel.transform.position = new Vector3(0, 0, (CurrentLevel.StageCount) * 100f + 60f); //world space length of the level is 100 per stage and 60 for final area
        }

        public static GameController _instance;
        public static GameController Instance => _instance = _instance ? _instance : _instance = FindObjectOfType<GameController>();
    }
}
