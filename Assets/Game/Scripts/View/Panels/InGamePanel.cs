using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Generics;
using Mek.Localization;
using MekNavigation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class InGamePanel : Panel
    {
        [SerializeField] private CurrencyElement _coinElement;
        [SerializeField] private Text _localizedText;

        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);

            InitializeElements();

            if (LocalizationManager.TryGetTranslationWithParameter("TIME LEFT", "time", "40", out string loc))
            {
                _localizedText.text = loc;
            }
        }

        public override void Close()
        {
            DisposeElements();

            base.Close();
        }

        private void InitializeElements()
        {
            _coinElement.Init(PlayerData.Instance.Coin);

            _localizedText.gameObject.SetActive(false);
        }

        private void DisposeElements()
        {

        }

        [Button]
        public void EarnMoney(float amount = 10)
        {
            PlayerData.Instance.Coin += amount;
            _coinElement.UpdateValue(PlayerData.Instance.Coin);
        }

    }
}