using DG.Tweening;
using Game.Scripts.Controllers;
using Mek.Extensions;
using Mek.Utilities;
using MekAudio;
using MekCoroutine;
using UnityEngine;

namespace Game.Scripts.Behaviours
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private ParticleSystem _popParticle;
        [SerializeField] private AudioClip _popSound;

        public void Collected()
        {
            CoroutineController.DoAfterGivenTime(Random.Range(0f,0.2f), () =>
            {
                Pop();
                Destroy(gameObject);
            });
        }

        private void Pop()
        {
            AudioController.Play(_popSound, volume:0.5f, position:transform.position, in3DSpace: true);
            var particle = _popParticle.Spawn(transform.position, Quaternion.identity);

            particle.Play(true);
        }
    }
}
