using System;
using System.Collections;
using DG.Tweening;
using Mek.Extensions;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Mek.Generics
{
    public class NumberAnimator
    {
        protected float Current;
        protected Text Text;
        public NumberAnimator(float current, Text text)
        {
            Text = text;
            SetCurrent(current);
        }

        public void SetCurrent(float current)
        {
            Current = current;
            Text.text = Current.ToString();
        }

        public void UpdateValue(float to, float duration = 0.5f)
        {
            DOTween.To(
                getter: () => Current,
                setter: SetCurrent,
                endValue: to,
                duration);
        }
    }
}
