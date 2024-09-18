using RicoShot.Core;
using RicoShot.Core.Interface;
using Zenject;

public class GameStateManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IGameStateManager>()
            .To<GameStateManager>()
            .AsSingle();
    }
}