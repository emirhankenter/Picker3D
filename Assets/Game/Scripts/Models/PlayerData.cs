using Mek.Models.Stats;
using System;
using System.Collections.Generic;
using Mek.Localization;
using Mek.Models;

namespace Game.Scripts.Models
{
    public class PlayerData : MekPlayerData
    {
        public static readonly Dictionary<string, BaseStat> Prefs = new Dictionary<string, BaseStat> {

            // MekPlayerData
            { PrefStats.IsStatSet, new BoolStat() },
            { PrefStats.SoundFXEnabled, new BoolStat(true) },

            // Game Specific PLayerData
            { PrefStats.PlayerLevel, new IntStat(0, Int32.MaxValue, 1) },
            { PrefStats.Language, new IntStat(0, Int32.MaxValue, 0) },
            { PrefStats.LastActive, new DateStat(DateTime.UtcNow) },
            { PrefStats.Coin, new FloatStat(0f, float.MaxValue, 0f) },
        };

        public DateTime LastActive
        {
            get => PrefsManager.GetDate(PrefStats.LastActive);
            set => PrefsManager.SetDate(PrefStats.LastActive, value);
        }

        public int PlayerLevel
        {
            get => PrefsManager.GetInt(PrefStats.PlayerLevel);
            set => PrefsManager.SetInt(PrefStats.PlayerLevel, value);
        }

        public float Coin
        {
            get => PrefsManager.GetFloat(PrefStats.Coin);
            set => PrefsManager.SetFloat(PrefStats.Coin, value);
        }

        public int Language
        {
            get => PrefsManager.GetInt(PrefStats.Language);
            set => PrefsManager.SetInt(PrefStats.Language, value);
        }

        #region Singleton

        private static PlayerData _instance = new PlayerData();

        public static PlayerData Instance => _instance;

        #endregion
    }
}