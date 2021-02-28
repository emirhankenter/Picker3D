using System;
using MekNavigation;
using UnityEngine;

namespace Game.Scripts.Models.ViewParams
{
    public class MainMenuParams : MekNavigation.ViewParams
    {
        public Action OnTap;
        public MainMenuParams(Action onTap) : base(ViewTypes.MainMenuPanel)
        {
            OnTap = onTap;
        }
    }
}
