using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Localization;
using MekNavigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Scripts.View.Panels
{
    public class GameOverPanel : Panel
    {
        [SerializeField] private CurrencyElement _coinElement;
        [SerializeField] private MovingCoinAnimator _coinAnimatorElement;
        [SerializeField] private Button _claimButton;
        [SerializeField] private RectTransform _rewardContainer;
        [SerializeField] private Text _rewardAmountText;
        [SerializeField] private Text _gameOverText;
        [SerializeField] private Text _claimButtonText;

        private GameOverViewParams _params;

        public override void Open(ViewParams viewParams)
        {
            _params = viewParams as GameOverViewParams;

            if (_params == null) return;

            InitializeElements();

            base.Open(viewParams);
        }

        public override void Close()
        {
            DisposeElements();

            base.Close();
        }

        private void InitializeElements()
        {
            _coinElement.Init(PlayerData.Instance.Coin);
            _claimButton.interactable = true;

            if (LocalizationManager.TryGetTranslation(_params.IsSuccess ? "LevelCompleted" : "LevelFail",
                out var levelFinishText))
            {
                _gameOverText.text = levelFinishText;
            }
            else
            {
                _gameOverText.text = _params.IsSuccess ? "LevelCompleted" : "LevelFailed";
            }

            if (LocalizationManager.TryGetTranslation(_params.IsSuccess ? "Claim" : "Try Again",
                out var buttonText))
            {
                _claimButtonText.text = buttonText;
            }
            else
            {
                _claimButtonText.text = _params.IsSuccess ? "Claim" : "Retry";
            }
            _rewardContainer.gameObject.SetActive(_params.IsSuccess);
            _rewardAmountText.text = _params.EarnAmount.ToString();
        }

        private void DisposeElements()
        {
        }

        [Button]
        public void OnClaimButtonClicked()
        {
            _claimButton.interactable = false;

            if (_params.IsSuccess)
            {
                _coinAnimatorElement.Move(onFirstCoinCollected: () =>
                    {
                        _coinElement.UpdateValue(PlayerData.Instance.Coin);
                    },
                    onLastCoinCollected: () =>
                    {
                        _params.RewardClaimed?.Invoke();
                    });
            }
            else
            {
                _params.RewardClaimed?.Invoke();
            }
        }
    }
}
