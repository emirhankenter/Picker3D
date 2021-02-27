using System;
using Mek.Extensions;
using MekCoroutine;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    public delegate void VerticalProgressDelegate();
    public class VerticalProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Image _arrow;
        
        private static readonly float _bottom = -99f;
        private static readonly float _top = 99f;

        private static event VerticalProgressDelegate _activated;
        private static event Action _stoped;

        private static float _progress;

        private bool _isActivated { get; set; }

        private void Awake()
        {
            _activated += OnActivated;
        }

        private void OnDestroy()
        {
            _activated -= OnActivated;
        }

        public static void Activate()
        {
            CoroutineController.DoAfterCondition(() => _activated != null, () =>
            {
                _activated?.Invoke();
            });
        }

        public static void Stop()
        {
            _stoped?.Invoke();
        }

        private void OnActivated()
        {
            _stoped += OnStoped;
            _progress = 0;
            Fill(_progress);

            _isActivated = true;
            _content.gameObject.SetActive(true);
        }

        private void OnStoped()
        {
            _stoped -= OnStoped;
            _isActivated = false;
            _content.gameObject.SetActive(false);
        }

        public static void UpdateValue(float progress)
        {
            _progress = progress;
        }

        private void FixedUpdate()
        {
            if (_isActivated)
            {
                Fill(_progress);
                var posY = _progress.Denormalize(_bottom, _top);
                SetPos(posY);
            }
        }

        private void SetPos(float posY)
        {
            _arrow.transform.localPosition = new Vector3(_arrow.transform.localPosition.x, posY, _arrow.transform.localPosition.z);
        }

        private void Fill(float progress)
        {
            _fillImage.fillAmount = progress;
        }
    }
}
