using Cysharp.Threading.Tasks;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using RicoShot.Title.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestTitleSceneManager : ITitleSceneManager, IInitializable
{
    public TitleInputs TitleInputs => throw new System.NotImplementedException();

    [Inject] IGameStateManager gameStateManager;

    public void Initialize()
    {
        UniTask.Create(async () =>
        {
            await UniTask.Delay(1000);
            gameStateManager.NextScene();
        });
    }
}
