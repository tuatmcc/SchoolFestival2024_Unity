using Zenject;

namespace Shooting_test
{
    public class TimeControllerINstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TimeController>().AsSingle();
        }
    }
}