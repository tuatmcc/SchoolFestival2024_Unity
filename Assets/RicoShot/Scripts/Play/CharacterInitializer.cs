using Cysharp.Threading.Tasks;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    public class CharacterInitializer : NetworkBehaviour
    {
        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Start()
        {
            // ここでZenAutoInjectorを付けることでうまくいく
            gameObject.AddComponent<ZenAutoInjecter>();
            SetUpCharacter().Forget();
        }

        private async UniTask SetUpCharacter()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager != null, cancellationToken: destroyCancellationToken);
            if (IsClient && IsOwner)
            {
                playSceneManager.LocalPlayer = gameObject;
            }
            if (IsServer && IsOwner)
            {
                // ここにNPC用のスクリプトを有効化するスクリプト
            }
        }
    }
}
