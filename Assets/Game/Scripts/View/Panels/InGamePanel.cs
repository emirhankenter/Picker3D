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
        [SerializeField] private StageIndicator _stageIndicator;

        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);

            InitializeElements();
        }

        public override void Close()
        {
            DisposeElements();

            base.Close();
        }

        private void InitializeElements()
        {
            _coinElement.Init(PlayerData.Instance.Coin);
            _stageIndicator.Init();
        }

        private void DisposeElements()
        {
            _stageIndicator.Dispose();
        }
    }
}