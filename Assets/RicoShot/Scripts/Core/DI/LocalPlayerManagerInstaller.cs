using RicoShot.Core.Tests;
using Zenject;

namespace RicoShot.Core.DI
{
    public class LocalPlayerManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<LocalPlayerManager>().AsSingle();
        }
    }
}
