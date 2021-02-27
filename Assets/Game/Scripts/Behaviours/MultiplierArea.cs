using System;
using Game.Scripts.Controllers;
using TMPro;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class MultiplierArea : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _groundRenderer;
        [SerializeField] private TextMeshPro _multiplyText;
        [SerializeField] private int _multiplyBy;

        public int MultiplyAmount => _multiplyBy;
        public bool Inside { get; private set; }

        private Color[] _colors = new[] {Color.green, Color.yellow, new Color(1, 0.5f, 0, 1), Color.red,};

        private void Awake()
        {
            _multiplyText.text = $"{_multiplyBy}X";
            _groundRenderer.material.color = _colors[_multiplyBy - 2];
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                Debug.Log($"{MultiplyAmount}X entered");
                Inside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                Debug.Log($"{MultiplyAmount}X exit");
                Inside = false;
            }
        }
    }
}
