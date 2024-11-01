using RicoShot.Play.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class PlaySceneTesterInstaller : MonoInstaller
    {
        [SerializeField] private PlaySceneTester playSceneTester;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlaySceneTester>().FromInstance(playSceneTester);
            if (playSceneTester.IsTest)
            {
                Container.BindInterfacesTo<DummyNetworkController>().AsSingle();
            }
        }
}
}
