using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Mek.Localization.Scripts
{
    public class Loc : MonoBehaviour
    {
        [SerializeField] private string _key;
        private Text _text;

        private void Awake()
        {
            _text = GetComponent<Text>();

            SetText();

            LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= OnLanguageChanged;
        }

        private void OnLanguageChanged()
        {
            SetText();
        }

        private void SetText()
        {
            if (LocalizationManager.TryGetTranslation(_key, out string translation))
            {
                _text.text = translation;
            }
        }
    }
}
