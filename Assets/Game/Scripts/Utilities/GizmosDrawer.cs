using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Scripts.Utilities
{
    public class GizmosDrawer : MonoBehaviour
    {
        public enum GizmosType
        {
            Box,
            Sphere
        }

        [SerializeField] private GizmosType _type;
        [SerializeField, ShowIf(nameof(_type), GizmosType.Box)] private BoxCollider _box;
        [SerializeField, ShowIf(nameof(_type), GizmosType.Sphere)] private SphereCollider _sphere;
        [SerializeField] private Color _color;
        private void OnDrawGizmos()
        {
            if (_box && _type == GizmosType.Box)
            {
                Gizmos.color = _color;
                Gizmos.DrawCube(_box.bounds.center, _box.bounds.size);
            }

            if (_sphere && _type == GizmosType.Sphere)
            {
                Gizmos.color = _color;
                Gizmos.DrawSphere(_sphere.bounds.center, _sphere.radius);
            }
        }
    }
}
