using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Models
{
    [CreateAssetMenu(menuName = "CollectibleData", fileName = "CollectibleData")]
    public class CollectibleData : ScriptableObject
    {
        public List<Vector3> Positions;
    }
}
