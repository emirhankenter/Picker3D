using Mek.Generics;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    public class CurrencyElement : MonoBehaviour
    {
        [SerializeField] private Text _currencyText;

        private NumberAnimator _moneyAnimator;
        public void Init(float current)
        {
            if (_moneyAnimator == null)
            {
                _moneyAnimator = new NumberAnimator(current, _currencyText);
            }
            else
            {
                _moneyAnimator.SetCurrent(current, roundToInt: true);
            }

            UpdateValue(current);
        }

        [Button]
        public void UpdateValue(float to = 1000)
        {
            _moneyAnimator.UpdateValue(to, roundToInt: true);
        }
    }
}
