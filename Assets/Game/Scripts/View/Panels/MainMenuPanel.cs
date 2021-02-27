using Game.Scripts.Controllers;
using MekNavigation;
using UnityEngine;

namespace Assets.Game.Scripts.View.Panels
{
    public class MainMenuPanel : Panel
    {
        public override void Open(ViewParams viewParams)
        {
            InputController.PressPerformed += OnPressPerformed;

            base.Open(viewParams);
        }

        public override void Close()
        {
            base.Close();
        }

        private void OnPressPerformed(Vector2 obj)
        {
            InputController.PressPerformed -= OnPressPerformed;

            Navigation.Panel.Change(ViewTypes.InGamePanel);
        }
    }
}
