using UnityEngine;
using Zenject;

namespace RicoShot.ModeSelect.DI
{
    public class ModeSelectSceneManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ModeSelectSceneManager>()
                .To<ModeSelectSceneManager>()
                .AsSingle();
        }
    }
}
