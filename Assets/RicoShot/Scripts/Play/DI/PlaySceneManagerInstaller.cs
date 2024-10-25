using RicoShot.Play;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlaySceneManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<PlaySceneManager>().AsSingle();
    }
}
