using Zenject;

namespace RicoShot.Play
{
    public class TimeControllerINstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<TimeController>().AsSingle();
        }
    }
}