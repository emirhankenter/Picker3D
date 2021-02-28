using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Models;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Scripts.Behaviours
{
    public class CollectibleStack : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged(nameof(PopularizeCollectibles))] private CollectibleData _collectibleData;

        [SerializeField] private Collectible _collectiblePrefab;

        private void PopularizeCollectibles()
        {
            var collectibles = transform.GetComponentsInChildren<Collectible>();

            collectibles.ForEach(collectible => DestroyImmediate(collectible.gameObject));

            if (_collectibleData == null) return;

            foreach (var position in _collectibleData.Positions)
            {
                var collectible = PrefabUtility.InstantiatePrefab(_collectiblePrefab, transform) as Collectible;
                collectible.transform.localPosition = position;
            }
        }

        [Button]
        private void InstantiateCollectible()
        {
            PrefabUtility.InstantiatePrefab(_collectiblePrefab, transform);
        }

        [Button]
        private void CreateAsset(string name = "CollectibleData")
        {
            var collectibleData = ScriptableObject.CreateInstance<CollectibleData>();

            var collectibles = transform.GetComponentsInChildren<Collectible>();

            collectibleData.Positions = new List<Vector3>();

            collectibleData.Positions.AddRange(collectibles.Select(collectible => collectible.transform.localPosition));

            AssetDatabase.CreateAsset(collectibleData, $"Assets/Game/Data/CollectibleData/{name}.asset");
            AssetDatabase.SaveAssets();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
#endif
    }
}
