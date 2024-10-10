using UnityEngine;
using Zenject;

public class ScoreManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IScoreManager>().To<ScoreManagerImpl>().AsSingle();
    }
}