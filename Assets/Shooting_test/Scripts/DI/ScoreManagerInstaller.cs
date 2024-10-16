using UnityEngine;
using Zenject;
namespace RicoShot.Play.DI
{
    public class ScoreManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IScoreManager>().To<ScoreManagerImpl>().AsSingle();
        }
    }
}