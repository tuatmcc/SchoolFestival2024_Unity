using UnityEngine;
using Zenject;
namespace RicoShot.Play
{
    public class ScoreManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IScoreManager>().To<ScoreManagerImpl>().AsSingle();
        }
    }
}