using UnityEngine;

namespace Shooting_test
{
    public interface IBulletObjectPoolManager
    {
        BulletController Shot(Vector3 position, Vector3 force, Quaternion rotation);
        BulletController Shot(Transform transform, Vector3 force);
        BulletController Shot();
    }
}