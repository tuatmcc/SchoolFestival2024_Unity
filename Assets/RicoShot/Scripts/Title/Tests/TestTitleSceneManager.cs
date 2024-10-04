using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using Zenject;

namespace RicoShot.Title.Tests
{
    public class TestTitleSceneManager : ITitleSceneManager, IInitializable
    {
        public TitleInputs TitleInputs => throw new System.NotImplementedException();

        [Inject] IGameStateManager gameStateManager;

        // 1秒でMatchingへ
        public void Initialize()
        {
            UniTask.Create(async () =>
            {
                await UniTask.Delay(1000);
                gameStateManager.NextScene();
            });
        }
    }
}
