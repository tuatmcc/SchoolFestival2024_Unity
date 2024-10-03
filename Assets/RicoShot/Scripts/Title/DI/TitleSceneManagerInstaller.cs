using Zenject;

public class TitleSceneManagerInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<TestTitleSceneManager>().AsSingle();
    }
}
