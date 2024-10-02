using RicoShot.Core;
using RicoShot.Core.Interface;
using Zenject;

namespace RicoShot.Core.DI
{
    public class GameStateManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<GameStateManager>()
                .AsSingle();
        }
    }
}
