using System;
using System.Collections.Generic;
using Game.Scripts.Models;
using Game.Scripts.View.Elements;
using Mek.Localization;
using MekNavigation;
using UnityEngine;

namespace Game.Scripts.View
{
    public class SettingsPopup : Popup
    {
        [SerializeField] private RectTransform _languagesParent;
        [SerializeField] private LanguageSelectionButton _languageSelectionButtonPrefab;

        private List<LanguageSelectionButton> _selectionButtons = new List<LanguageSelectionButton>();

        private bool _isClosing;
        public override void Open(ViewParams viewParams)
        {
            InitializeElements();
            _isClosing = false;

            base.Open(viewParams);
        }

        public override void Close()
        {
            if (!_isClosing)
            {
                _isClosing = true;
            }
            else
            {
                return;
            }

            base.Close();
        }

        private void InitializeElements()
        {
            if (_languagesParent.childCount == 0)
            {
                foreach (string name in Enum.GetNames(typeof(Language)))
                {
                    if (Enum.TryParse(name, out Language lang))
                    {
                        var button = Instantiate(_languageSelectionButtonPrefab, _languagesParent);
                        button.Init(lang);
                        button.Clicked += OnLanguageButtonClicked;
                        _selectionButtons.Add(button);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            foreach (var button in _selectionButtons)
            {
                button.Clicked -= OnLanguageButtonClicked;
                Destroy(button.gameObject);
            }
        }

        private void OnLanguageButtonClicked(Language lang)
        {
            _selectionButtons.ForEach(button => button.Check());
        }

        public void OnClickedCloseButton()
        {
            Navigation.Popup.CloseActiveContent();
        }
    }
}
