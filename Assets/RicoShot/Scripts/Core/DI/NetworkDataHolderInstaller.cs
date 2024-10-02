using RicoShot.Core;
using System;
using UnityEngine;
using Zenject;

public class NetworkDataHolderInstaller : MonoInstaller
{
    [SerializeField] private NetworkDataHolder networkDataHolder;

    public override void InstallBindings()
    {
        Container.BindInterfacesTo<NetworkDataHolder>().FromInstance(networkDataHolder).AsSingle();
    }
}
