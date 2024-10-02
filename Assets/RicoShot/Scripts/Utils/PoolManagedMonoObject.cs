using UnityEngine;

namespace RicoShot.Utils
{
    public abstract class PoolManagedMonoObject : MonoBehaviour
    {
        public virtual void Init() { }
        
        public virtual void Deactivate() { }
    }
}