using RicoShot.Play.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class TimeManagerInstaller : MonoInstaller
    {
        [SerializeField] TestTimeManager timeManager;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestTimeManager>().FromInstance(timeManager);
        }
    }
}
