using System;
using Game.Scripts.Models;
using UnityEngine;
using Mek.Models;

namespace Mek.Controllers
{
    public class GameBaseController : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (!MekPlayerData.IsStatSet)
            {
                InitializePlayerPrefs();
            }
            else
            {
                InitializeUnsetPlayerPrefs();
            }
        }

        protected void InitializePlayerPrefs()
        {
            foreach (var stat in PlayerData.Prefs)
            {
                var type = stat.Value.GetStatType();

                PrefsManager.SetByType(stat.Key, type);
            }

            MekPlayerData.IsStatSet = true;
            Debug.LogWarning("Prefs initialized");
        }

        protected void InitializeUnsetPlayerPrefs()
        {
            foreach (var stat in PlayerData.Prefs)
            {
                if (!PrefsManager.HasKey(stat.Key))
                {
                    var type = stat.Value.GetStatType();

                    PrefsManager.SetByType(stat.Key, type);
                }
            }
        }
    }
}