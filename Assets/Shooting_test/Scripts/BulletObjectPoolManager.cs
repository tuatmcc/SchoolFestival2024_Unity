using RicoShot.Utils;
using UnityEngine;

namespace Shooting_test
{
    public class BulletObjectPoolManager : ObjectPoolManager<BulletController>, IBulletObjectPoolManager
    {
        public BulletObjectPoolManager(BulletController prefab) : base(prefab)
        {
        }
        
        public BulletController Shot(Vector3 position, Vector3 force, Quaternion rotation)
        {
            var bullet = GetObject();
            bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.GetComponent<Rigidbody>().AddForce(force);
            return bullet;
        }

        public BulletController Shot(Transform transform, Vector3 force)
        {
            var bullet = GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.GetComponent<Rigidbody>().AddForce(force);
            return bullet;
        }

        public BulletController Shot()
        {
            return GetObject();
        }
    }
}