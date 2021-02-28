using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.Elements
{
    public class StageIndicatorElement : MonoBehaviour
    {
        [SerializeField] private Image _completeImage;

        public void ToggleState(bool state)
        {
            _completeImage.enabled = state;
        }
    }
}
