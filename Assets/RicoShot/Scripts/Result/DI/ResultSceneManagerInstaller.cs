using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneManagerInstaller : MonoInstaller<ResultSceneManagerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ResultSceneManager>().AsSingle();
        }
    }
}