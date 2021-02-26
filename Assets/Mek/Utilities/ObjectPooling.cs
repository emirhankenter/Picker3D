using System.Collections.Generic;
using System.Linq;
using Mek.Interfaces;
using UnityEngine;

namespace Mek.Utilities
{
    public class ObjectPooling : MonoBehaviour
    {
        private Transform _poolParent;
        private Dictionary<int, List<MonoBehaviour>> _pool;

        protected void Init()
        {
            _poolParent = transform;
            _pool = new Dictionary<int, List<MonoBehaviour>>();
        }

        public T Spawn<T>(T prefab) where T : MonoBehaviour
        {
            var hashCode = prefab.GetHashCode();
            var result = GetObj(hashCode);
            MonoBehaviour obj;
            if (result == null)
            {
                obj = Instantiate(prefab);
                if (!_pool.ContainsKey(hashCode))
                {
                    _pool.Add(hashCode, new List<MonoBehaviour>{obj});
                }
                else
                {
                    _pool[prefab.GetHashCode()].Add(obj);
                }
            }
            else
            {
                obj = result;
                obj.gameObject.SetActive(true);
                obj.transform.SetParent(null, true);
            }

            return obj as T;
        }

        public void Recycle<T>(T obj) where T : MonoBehaviour
        {
            obj.transform.SetParent(_poolParent, true);
            obj.gameObject.SetActive(false);

            obj.TryGetComponent(out IRecyclable recyclable);
            recyclable?.Recycle();
        }

        private MonoBehaviour GetObj(int hashCode)
        {
            if (_pool.ContainsKey(hashCode))
            {
                var result = _pool[hashCode]
                    .Where(item => !item.gameObject.activeSelf && item.transform.IsChildOf(_poolParent))
                    .ToList();

                return result.Count > 0 ? result[0] : null;
            }

            return null;
        }

        private static ObjectPooling _instance;

        public static ObjectPooling Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject("ObjectPool").AddComponent<ObjectPooling>();
                    _instance.Init();
                }

                return _instance;
            }
        }
    }
}
