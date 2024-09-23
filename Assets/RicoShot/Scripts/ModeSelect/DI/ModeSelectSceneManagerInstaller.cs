using UnityEngine;
using Zenject;

public class ModeSelectSceneManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<ModeSelectSceneManager>()
            .To<ModeSelectSceneManager>()
            .AsSingle();
    }
}