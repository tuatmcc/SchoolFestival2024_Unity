using RicoShot.Play;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.DI
{
    public class PlaySceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlaySceneManager>().AsSingle();
        }
    }
}
