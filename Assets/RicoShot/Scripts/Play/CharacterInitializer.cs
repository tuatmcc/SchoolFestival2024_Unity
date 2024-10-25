using Chibi;
using Cysharp.Threading.Tasks;
using RicoShot.Core;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    [RequireComponent(typeof(LocalPlayerMoveController))]
    public class CharacterInitializer : NetworkBehaviour
    {
        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Start()
        {
            // ここでZenAutoInjectorを付けることでうまくいく
            gameObject.AddComponent<ZenAutoInjecter>();
            SetUpCharacter().Forget();
        }

        // SpawnとInjectが終わるのを待ってからセッティングを開始
        private async UniTask SetUpCharacter()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager != null, cancellationToken: destroyCancellationToken);
            if (IsClient && IsOwner)
            {
                playSceneManager.LocalPlayer = gameObject;
            }
            else if (IsServer && IsOwner)
            {

            }
            else
            {

            }
            Debug.Log("Initialized character");
        }

        public void SetCharacterParams(CharacterParams characterParams)
        {
            ReflectCharacterParamsAsync(characterParams).Forget();
        }

        // 自身の見た目を反映させたうえで、クライアントにも反映させる関数
        private async UniTask ReflectCharacterParamsAsync(CharacterParams characterParams)
        {
            ReflectCharacterParams(characterParams);
            await UniTask.WaitUntil(() => IsSpawned, cancellationToken: destroyCancellationToken);
            SendCharaterParamsRpc(characterParams);
        }

        // (サーバー→クライアント)サーバーからクライアントにCharacterParamsを送信して反映する関数
        [Rpc(SendTo.NotServer)]
        private void SendCharaterParamsRpc(CharacterParams characterParams)
        {
            ReflectCharacterParams(characterParams);
        }

        private void ReflectCharacterParams(CharacterParams characterParams)
        {
            var characterSettingController = GetComponent<CharacterSettingsController>();
            characterSettingController.activeChibiIndex = characterParams.ChibiIndex;
            characterSettingController.hairColor = characterParams.HairColor.ToString();
            characterSettingController.costumeVariant = characterParams.CostumeVariant;
            characterSettingController.accessory = characterParams.Accessory;
        }
    }
}
