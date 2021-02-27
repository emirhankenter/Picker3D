using DG.Tweening;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        public void Collected()
        {
            transform.DOScale(Vector3.zero, 1f).SetEase(Ease.Linear);
        }
    }
}
