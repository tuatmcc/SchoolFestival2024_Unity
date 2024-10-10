using Zenject;

namespace Shooting_test.DI
{
    public class TimeControllerINstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TimeController>().AsSingle();
        }
    }
}