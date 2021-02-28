using DG.Tweening;
using Game.Scripts.Controllers;
using Game.Scripts.Models.ViewParams;
using Game.Scripts.Utilities;
using MekCoroutine;
using MekNavigation;
using UnityEngine;

namespace Assets.Game.Scripts.View.Panels
{
    public class MainMenuPanel : Panel
    {
        [SerializeField] private RectTransform _handImage;

        public override void Open(ViewParams viewParams)
        {
            CoroutineController.DoAfterCondition(() => GameController.Instance.DragHandler.IsActive, () =>
            {
                DragHandler.PointerDown += OnPressPerformed;
                InitializeElements();
            });

            base.Open(viewParams);
        }

        public override void Close()
        {
            base.Close();

            DisposeElements();
        }

        private void InitializeElements()
        {
            _handImage.DOScale(_handImage.localScale * 1.3f, 0.5f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        }
        private void OnPressPerformed(Vector2 obj)
        {
            //InputController.PressPerformed -= OnPressPerformed;
            DragHandler.PointerDown -= OnPressPerformed;

            Navigation.Panel.Change(ViewTypes.InGamePanel);
        }

        private void DisposeElements()
        {
            _handImage.DOKill();
            _handImage.localScale = Vector3.one;
        }

        public void OnSettingsButtonClicked()
        {
            Navigation.Popup.Open(ViewTypes.SettingsPopup);
        }
    }
}
