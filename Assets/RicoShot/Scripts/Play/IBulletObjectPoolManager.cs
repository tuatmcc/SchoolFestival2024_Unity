using UnityEngine;

namespace RicoShot.Play
{
    public interface IBulletObjectPoolManager
    {
        BulletController Shot(Vector3 position, Vector3 force, Quaternion rotation);
        BulletController Shot(Transform transform, Vector3 force);
        BulletController Shot();
    }
}