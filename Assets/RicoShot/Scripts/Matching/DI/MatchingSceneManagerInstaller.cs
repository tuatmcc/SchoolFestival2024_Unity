using RicoShot.Matching.Tests;
using Zenject;

namespace RicoShot.Matching.DI
{
    public class MatchingSceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MatchingSceneManager>().AsSingle();
        }
    }
}