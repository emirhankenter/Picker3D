using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utilities
{
    public class CrossfadeTransition : MonoBehaviour
    {
        private static Image _image;

        private static Color _fadeOutColor = new Color(20f/255f, 20f/255f, 20f/255f, 0f);
        private static Color _fadeInColor = new Color(20f/255f, 20f/255f, 20f/255f, 1f);

        private static void Init()
        {
            _image = Instance.GetComponent<Image>();
        }
        public static void FadeIn(float timer, Action callBack)
        {
            if (!Instance) return;
            _image.enabled = true;

            _image.DOColor(_fadeInColor, timer)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    callBack?.Invoke();
                });
        }
        public static void FadeOut(float timer, Action callBack)
        {
            if (!Instance) return;

            _image.enabled = true;
            _image.color = _fadeInColor;

            _image.DOColor(_fadeOutColor, timer)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    callBack?.Invoke();
                    _image.enabled = false;
                });
        }

        private static CrossfadeTransition _instance;

        public static CrossfadeTransition Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<CrossfadeTransition>();
                    Init();
                }

                return _instance;
            }
        }
    }
}
