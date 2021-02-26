using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MekAudio;
using UnityEngine;

namespace MekNavigation
{
    public class Popup : ContentBase
    {
        [SerializeField] private AudioClip _openingSound;
        [SerializeField] private AudioClip _closingSound;
        public override void Open(ViewParams viewParams)
        {
            transform.localScale = Vector3.zero;
            base.Open(viewParams);
            if (_openingSound)
            {
                AudioController.Play(_openingSound);
            }
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }

        public override void Close()
        {
            if (_closingSound)
            {
                AudioController.Play(_closingSound);
            }
            transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
                gameObject.SetActive(false);
                Closed?.Invoke();
            });
        }
    }
}