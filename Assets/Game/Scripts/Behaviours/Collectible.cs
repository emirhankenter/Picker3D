using System;
using DG.Tweening;
using Game.Scripts.Controllers;
using Game.Scripts.Models;
using Mek.Extensions;
using Mek.Models;
using MekAudio;
using MekCoroutine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Behaviours
{
    public class Collectible : MonoBehaviour, ICollectible
    {
        [SerializeField] private ParticleSystem _popParticle;
        [SerializeField] private AudioClip _popSound;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Collected()
        {
            CoroutineController.DoAfterGivenTime(Random.Range(0f,0.2f), () =>
            {
                Pop();

                CoroutineController.DoAfterFixedUpdate(() => Destroy(gameObject));
            });
        }

        private void Pop()
        {
            if (MekPlayerData.SoundFXEnabled)
            {
                AudioController.Play(_popSound, volume: 0.5f, position: transform.position, in3DSpace: true);
            }

            var particle = _popParticle.Spawn(transform.position, Quaternion.identity);

            particle.Play(true);
        }

        public void PushForward()
        {
            _rb.AddForce(Vector3.forward * 5f);
        }

        public void Bounce()
        {
            _rb.AddForce(Vector3.up * 0.1f + Vector3.back * 0.1f);
        }
    }
}
