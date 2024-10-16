using UnityEngine;
using Zenject;
namespace Shooting_test
{
    public class ScoreManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IScoreManager>().To<ScoreManagerImpl>().AsSingle();
        }
    }
}