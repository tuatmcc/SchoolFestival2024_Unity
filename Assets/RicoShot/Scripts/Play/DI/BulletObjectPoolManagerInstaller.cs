using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class BulletObjectPoolManagerInstaller : MonoInstaller
    {
        [SerializeField] private BulletController bulletPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<IBulletObjectPoolManager>()
                .FromInstance(new BulletObjectPoolManager(bulletPrefab))
                .AsSingle();
        }
    }
}