using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace RicoShot.Utils
{
    public abstract class ObjectPoolManager<T> where T : PoolManagedMonoObject
    {
        protected T Prefab;
        private bool _collectionCheck = true;
        private int _defaultCapacity = 32;
        private int _maxCapacity = 128;
        
        private IObjectPool<T> _objectPool;
        
        public ObjectPoolManager(T prefab, bool collectionCheck = true, int defaultCapacity = 32, int maxCapacity = 128)
        {
            Prefab = prefab;
            _collectionCheck = collectionCheck;
            _defaultCapacity = defaultCapacity;
            _maxCapacity = maxCapacity;
            
            _objectPool = new ObjectPool<T>(
                () => Object.Instantiate<T>(Prefab),
                obj => {
                    obj.gameObject.SetActive(true);
                    obj.Init();
                },
                obj => {
                    obj.gameObject.SetActive(false);
                    obj.Deactivate();
                },
                Object.Destroy,
                _collectionCheck,
                _defaultCapacity,
                _maxCapacity);
        }
        
        protected T GetObject()
        {
            return _objectPool.Get();
        }
    }
}