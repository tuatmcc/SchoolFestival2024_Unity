using UnityEngine;
using UnityEngine.Pool;

namespace RicoShot.Utils
{
    public abstract class PoolManagedMonoObject : MonoBehaviour
    {
        private IObjectPoolManager<PoolManagedMonoObject> _objectPoolManager;
        
        public void Initialize(IObjectPoolManager<PoolManagedMonoObject> objectPoolManager)
        {
            _objectPoolManager = objectPoolManager;
        }
        
        // Start みたいなもん
        public virtual void Activate() { }
        
        // OnDestroy みたいなもん
        public virtual void Deactivate() { }
        
        // Destroy みたいなもん
        public void ReturnPool() { 
            _objectPoolManager.Release(this);
        }
    }
}