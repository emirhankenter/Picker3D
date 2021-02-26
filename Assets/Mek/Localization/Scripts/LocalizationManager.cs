using System;
using Mek.Models;
using UnityEngine;

namespace Mek.Localization
{
    public static class LocalizationManager
    {
        public static event Action LanguageChanged; 

        private static Localization _localization;
        public static Language Language { get; private set; }

        private static MekLog Log = new MekLog(nameof(LocalizationManager), DebugLevel.Debug);

        public static void Init()
        {
            _localization = Resources.Load<Localization>("Localization");
        }

        public static void SetLanguage(Language language)
        {
            Language = language;
            LanguageChanged?.Invoke();
        }

        public static bool TryGetTranslation(string key, out string translation)
        {
            translation = "";
            if (_localization.Dictionary.TryGetValue(key, out Localization.LanguageDictionary langDict))
            {
                if (langDict.TryGetValue(Language, out translation))
                {
                    return true;
                }
                else
                {
                    Log.Error($"Translation does not exists!");
                    return false;
                }
            }
            else
            {
                Log.Error($"Translation does not exists!");
                return false;
            }
        }

        public static bool TryGetTranslationWithParameter(string key, string parameter, string value, out string loc)
        {
            loc = "";
            if (TryGetTranslation(key, out var translation))
            {
                loc = translation.Replace("{" + parameter + "}", value);

                return true;
            }

            return false;
        }
    }
}
