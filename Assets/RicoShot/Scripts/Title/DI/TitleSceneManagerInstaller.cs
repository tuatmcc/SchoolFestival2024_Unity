using RicoShot.Title.Tests;
using Zenject;

namespace RicoShot.Title.DI
{
    public class TitleSceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestTitleSceneManager>().AsSingle();
        }
    }
}
