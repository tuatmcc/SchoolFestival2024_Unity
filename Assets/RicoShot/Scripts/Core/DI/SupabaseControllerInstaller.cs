using RicoShot.Core.Tests;
using Zenject;

namespace RicoShot.Core.DI
{
    public class SupabaseControllerInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TestSupabaseController>().AsSingle();
        }
    }
}
