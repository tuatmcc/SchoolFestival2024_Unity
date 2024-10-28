using RicoShot.Play.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class NetworkScoreManagerInstaller : MonoInstaller
    {
        [SerializeField] private TestNetworkScoreManager scoreManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestNetworkScoreManager>().FromInstance(scoreManager);
        }
    }
}
