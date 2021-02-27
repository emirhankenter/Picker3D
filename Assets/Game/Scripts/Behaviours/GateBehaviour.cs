using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class GateBehaviour : MonoBehaviour
    {
        [SerializeField] private Transform _rightHinge;
        [SerializeField] private Transform _leftHinge;

        public void OpenSesame(Action onOpened)
        {
            _rightHinge.DOLocalRotate(new Vector3(0, 0, -80f), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear);
            _leftHinge.DOLocalRotate(new Vector3(0, 0, -80f), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).OnComplete(
                () =>
                {
                    onOpened?.Invoke();
                });
        }
    }
}
