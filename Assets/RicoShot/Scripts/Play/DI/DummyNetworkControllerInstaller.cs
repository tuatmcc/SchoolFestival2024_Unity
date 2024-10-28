using RicoShot.Core.Interface;
using RicoShot.Play.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DummyNetworkControllerInstaller : MonoInstaller
{
    [SerializeField] private PlaySceneTester playSceneTester;

    public override void InstallBindings()
    {
        if (playSceneTester.IsTest)
        {
            Container.BindInterfacesTo<DummyNetworkController>().AsSingle();
        }
    }
}
