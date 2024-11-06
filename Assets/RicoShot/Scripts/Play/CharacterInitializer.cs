using Chibi;
using Cysharp.Threading.Tasks;
using RicoShot.Core;
using RicoShot.Play.Interface;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    [RequireComponent(typeof(LocalPlayerMoveController))]
    public class CharacterInitializer : NetworkBehaviour, IClientDataHolder
    {
        public ClientData ClientData { get; private set; }

        private BehaviorParameters behaviorParameters;
        private AgentPlayer agentPlayer;
        private RayPerceptionSensorComponent3D rayPerceptionSensor;
        private DecisionRequester decisionRequester;

        [SerializeField] private PlayerTeamColorIndicator playerTeamColorIndicator;

        [Inject] private readonly IPlaySceneManager playSceneManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        private void Awake()
        {
            behaviorParameters = GetComponent<BehaviorParameters>();
            agentPlayer = GetComponent<AgentPlayer>();
            rayPerceptionSensor = GetComponent<RayPerceptionSensorComponent3D>();
            decisionRequester = GetComponent<DecisionRequester>();
            behaviorParameters.enabled = false;
            agentPlayer.enabled = false;
            rayPerceptionSensor.enabled = false;
            decisionRequester.enabled = false;
        }

        private void Start()
        {
            // ここでZenAutoInjectorを付けることでうまくいく
            gameObject.AddComponent<ZenAutoInjecter>();

            if (playSceneTester.IsTest)
            {
                playSceneManager.LocalPlayer = gameObject;
                ClientData.CharacterParams.OnDataChanged += OnCharacterParamsChanged;
                NetworkObject.SynchronizeTransform = false;
                var rb = GetComponent<Rigidbody>();
                rb.isKinematic = false;
                if (playSceneTester.BehaveAsNPC)
                {
                    behaviorParameters.enabled = true;
                    agentPlayer.enabled = true;
                    rayPerceptionSensor.enabled = true;
                    decisionRequester.enabled = true;
                }

                return;
            }

            SetUpCharacter().Forget();
        }

        // SpawnとInjectが終わるのを待ってからセッティングを開始
        private async UniTask SetUpCharacter()
        {
            await UniTask.WaitUntil(() => IsSpawned && playSceneManager != null,
                cancellationToken: destroyCancellationToken);
            if (IsClient && IsOwner)
            {
                playSceneManager.LocalPlayer = gameObject;
            }
            else if (IsServer && IsOwner)
            {
                behaviorParameters.enabled = true;
                agentPlayer.enabled = true;
                rayPerceptionSensor.enabled = true;
                decisionRequester.enabled = true;
            }
            else
            {
            }

            if (IsServer) playSceneManager.OnPlayStateChanged += DestroyInServer;

            UniTask.Create(async () =>
            {
                await UniTask.WaitUntil(() => ClientData != null, cancellationToken: destroyCancellationToken);
                behaviorParameters.TeamId = ClientData.Team == Team.Alpha ? 0 : 1;
            }).Forget();
            Debug.Log("Initialized character");
        }

        // (サーバー)クライアントのデータをセットする関数
        public void SetCharacterParams(ClientData clientData)
        {
            ClientData = clientData;
            tag = $"{ClientData.Team}Character";
            ReflectCharacterParamsAsync(clientData).Forget();
        }

        // 自身の見た目を反映させたうえで、クライアントにも反映させる関数
        private async UniTask ReflectCharacterParamsAsync(ClientData clientData)
        {
            ReflectCharacterParams(clientData);
            await UniTask.WaitUntil(() => IsSpawned, cancellationToken: destroyCancellationToken);
            SendCharaterParamsRpc(clientData);
        }

        // (サーバー→クライアント)サーバーからクライアントにCharacterParamsを送信して反映する関数
        [Rpc(SendTo.NotServer)]
        private void SendCharaterParamsRpc(ClientData clientData)
        {
            ClientData = clientData;
            ReflectCharacterParams(clientData);
        }

        private void ReflectCharacterParams(ClientData clientData)
        {
            var characterSettingController = GetComponent<CharacterSettingsController>();
            characterSettingController.activeChibiIndex = clientData.CharacterParams.ChibiIndex;
            characterSettingController.hairColor = clientData.CharacterParams.HairColor.ToString();
            characterSettingController.costumeVariant = clientData.CharacterParams.CostumeVariant;
            characterSettingController.accessory = clientData.CharacterParams.Accessory;
            playerTeamColorIndicator.SetTeamColor(clientData.Team);
        }

        private void OnCharacterParamsChanged()
        {
            ReflectCharacterParams(ClientData);
        }

        // (サーバー)リザルトへ移動時に破棄する関数
        private void DestroyInServer(PlayState playState)
        {
            if (playState == PlayState.Despawn) Destroy(gameObject);
        }
    }
}