﻿namespace Mek.Models
{
    public class MekPlayerData
    {
        public static bool IsStatSet
        {
            get => PrefsManager.GetBool(PrefStats.IsStatSet);
            set => PrefsManager.SetBool(PrefStats.IsStatSet, value);
        }

        public static bool SoundFXEnabled
        {
            get => PrefsManager.GetBool(PrefStats.SoundFXEnabled);
            set => PrefsManager.SetBool(PrefStats.SoundFXEnabled, value);
        }
    }
}
