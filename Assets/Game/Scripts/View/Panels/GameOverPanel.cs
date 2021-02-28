using Game.Scripts.Models;
using Game.Scripts.View.Elements;
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
            _gameOverText.text = _params.IsSuccess ? "Success" : "Fail";
            _claimButtonText.text = _params.IsSuccess ? "Claim" : "Retry";
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
