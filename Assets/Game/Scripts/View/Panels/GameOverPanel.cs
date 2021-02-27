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
        [SerializeField] private Button _claimButton;
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
        }

        private void DisposeElements()
        {
        }

        [Button]
        public void OnClaimButtonClicked()
        {
            _claimButton.interactable = false;
            Debug.Log($"Current: {PlayerData.Instance.Coin}, After: {PlayerData.Instance.Coin + _params.EarnAmount}");
            _coinElement.UpdateValue(PlayerData.Instance.Coin);
            _params.RewardClaimed?.Invoke();
        }
    }
}
