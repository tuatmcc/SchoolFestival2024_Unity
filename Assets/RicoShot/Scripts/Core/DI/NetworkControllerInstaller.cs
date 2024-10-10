using RicoShot.Core;
using System;
using UnityEngine;
using Zenject;

namespace RicoShot.Core.DI
{
    public class NetworkControllerInstaller : MonoInstaller
    {
        [SerializeField] private NetworkController networkController;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NetworkController>().FromInstance(networkController).AsSingle();
        }
    }
}
