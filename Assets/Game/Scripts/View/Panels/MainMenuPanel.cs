using DG.Tweening;
using Game.Scripts.Controllers;
using MekNavigation;
using UnityEngine;

namespace Assets.Game.Scripts.View.Panels
{
    public class MainMenuPanel : Panel
    {
        [SerializeField] private RectTransform _handImage;

        public override void Open(ViewParams viewParams)
        {
            base.Open(viewParams);
            InputController.PressPerformed += OnPressPerformed;
            InitializeElements();
        }

        public override void Close()
        {
            base.Close();

            DisposeElements();
        }

        private void OnPressPerformed(Vector2 obj)
        {
            InputController.PressPerformed -= OnPressPerformed;

            Navigation.Panel.Change(ViewTypes.InGamePanel);
        }

        private void InitializeElements()
        {
            _handImage.DOScale(_handImage.localScale * 1.3f, 0.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        }

        private void DisposeElements()
        {
            _handImage.DOKill();
            _handImage.localScale = Vector3.one;
        }
    }
}
