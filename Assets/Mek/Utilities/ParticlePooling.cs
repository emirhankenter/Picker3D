using System.Collections.Generic;
using UnityEngine;

namespace Mek.Utilities
{
    public class ParticlePooling : MonoBehaviour
    {
        private List<ParticleSystem> _particles = new List<ParticleSystem>();

        private Transform _pooledObjects;

        private void Awake()
        {
            var go = new GameObject("Pooled Objects");
            _pooledObjects = go.transform;
        }

        public ParticleSystem Spawn(ParticleSystem particle)
        {
            var selectedParticle = default(ParticleSystem);

            foreach (var obj in _particles)
            {
                if (!obj.gameObject.activeSelf && particle.main.ToString() == obj.main.ToString() && particle.name == obj.name)
                {
                    selectedParticle = obj;
                    break;
                }
            }

            if (selectedParticle == null)
            {
                var go = Instantiate(particle);
                go.name = go.name.Replace("(Clone)", "");
                _particles.Add(go);

                selectedParticle = go;
            }
            else
            {
                selectedParticle.transform.SetParent(null, true);
            }
            selectedParticle.gameObject.SetActive(true);

            return selectedParticle;
        }

        public ParticleSystem Spawn(ParticleSystem particle, Transform t)
        {
            var pose = new Pose(particle.transform.localPosition, particle.transform.rotation);
            var selectedParticle = Spawn(particle);
            selectedParticle.transform.SetParent(t, false);
            selectedParticle.transform.localPosition = pose.position;
            selectedParticle.transform.localRotation = pose.rotation;
            return selectedParticle;
        }

        public ParticleSystem Spawn(ParticleSystem particle, Vector3 position, Quaternion rotation, bool keepPrefabsInitialRotation = false)
        {
            var rotationDifference = particle.transform.rotation * Quaternion.Inverse(rotation);
            var selectedParticle = Spawn(particle);
            selectedParticle.transform.position = position;
            selectedParticle.transform.rotation = keepPrefabsInitialRotation ? rotationDifference : rotation;
            return selectedParticle;
        }

        public void Recycle(ParticleSystem particle)
        {
            particle.Stop();
            particle.Clear();
            particle.transform.SetParent(_pooledObjects);
            particle.gameObject.SetActive(false);
        }

        private static ParticlePooling _instance;
        public static ParticlePooling Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ParticlePooling>();

                    if (_instance == null)
                    {
                        var go = new GameObject(nameof(ParticlePooling));
                        var instance = go.AddComponent<ParticlePooling>();
                        _instance = instance;

                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }
    }
}