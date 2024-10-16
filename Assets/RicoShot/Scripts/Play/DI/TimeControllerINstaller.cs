using Zenject;

namespace RicoShot.Play.DI
{
    public class TimeControllerINstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TimeController>().AsSingle();
        }
    }
}