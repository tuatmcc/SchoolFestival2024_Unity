using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace RicoShot.Utils
{
    public abstract class ObjectPoolManager<T> : IObjectPoolManager<T> where T : PoolManagedMonoObject // ObjectPoolManager<PoolManagedMonoObject>
    {
        private readonly int _defaultCapacity = 8;
        private readonly int _maxCapacity = 16;
        private readonly IObjectPool<T> _objectPool;

        protected ObjectPoolManager(T prefab, bool collectionCheck = true, int defaultCapacity = 32, int maxCapacity = 128)
        {
            _defaultCapacity = defaultCapacity;
            _maxCapacity = maxCapacity;
            
            _objectPool = new ObjectPool<T>(
                () =>
                {
                    var obj = Object.Instantiate<T>(prefab);
                    obj.Initialize(this);
                    return obj;
                },
                obj => {
                    obj.gameObject.SetActive(true);
                    obj.Activate();
                },
                obj => {
                    obj.gameObject.SetActive(false);
                    obj.Deactivate();
                },
                Object.Destroy,
                collectionCheck,
                _defaultCapacity,
                _maxCapacity);
        }

        public T Get()
        {
            return _objectPool.Get();
        }

        public void Release(PoolManagedMonoObject obj)
        {
            if (obj is T monoObject)
            {
                _objectPool.Release(monoObject);
            }
            else
            {
                Debug.LogError($"Invalid object type between {obj.GetType()} and {typeof(T)}");
            }
        }
    }
}