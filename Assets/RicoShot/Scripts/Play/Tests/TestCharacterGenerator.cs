using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestCharacterGenerator : MonoBehaviour, ICharacterGenerator
    {
        [SerializeField] private NetworkObject networkObject;
        [SerializeField] private Transform[] alphaSpawnPoints;
        [SerializeField] private Transform[] bravoSpawnPoints;

        private static int TeamAlphaNum = 4;
        private static int TeamBravoNum = 4;
        private int _alphaSpawnIndex = 0;
        private int _bravoSpawnIndex = 0;
        private int TeamAlphaCount = 0;
        private int TeamBravoCount = 0;

        [Inject] private readonly INetworkController networkController;
        [Inject] private readonly INetworkScoreManager scoreManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        public List<Transform> PlayerTransforms { get; } = new();

        // Startでないとクライアント側のInjectがうまくいかない(ライフサイクルの関係だと思われる)
        // (サーバー)クライアントのプレイヤーとNPCを生成
        private void Start()
        {
            if (playSceneTester.IsTest)
            {
                var player = Instantiate(networkObject, Vector3.zero, Quaternion.identity);
                var initializer = player.GetComponent<CharacterInitializer>();
                initializer.SetCharacterParams(new ClientData(playSceneTester.CharacterParams));
                return;
            }

            // サーバー側のみの処理
            if (!NetworkManager.Singleton.IsServer) return;

            foreach (var clientData in networkController.ClientDataList) SpawnPlayer(clientData);

            while (TeamAlphaCount < TeamAlphaNum) SpawnNpc(Team.Alpha);

            while (TeamBravoCount < TeamBravoNum) SpawnNpc(Team.Bravo);
        }

        private void SpawnPlayer(ClientData clientData)
        {
            // index will not exceed the length of the array
            var pos = clientData.Team == Team.Alpha
                ? alphaSpawnPoints[_alphaSpawnIndex++].position
                : bravoSpawnPoints[_bravoSpawnIndex++].position;

            var player = Instantiate(networkObject, pos, Quaternion.identity);
            PlayerTransforms.Add(player.transform);

            TeamAlphaCount += clientData.Team == Team.Alpha ? 1 : 0;
            TeamBravoCount += clientData.Team == Team.Bravo ? 1 : 0;

            player.DontDestroyWithOwner = true;

            var initializer = player.GetComponent<CharacterInitializer>();
            initializer.SetCharacterParams(clientData);

            player.SpawnAsPlayerObject(clientData.ClientID);
            Debug.Log($"Created character: {clientData.ClientID}");

            scoreManager.RegistCharacter(clientData.UUID, clientData.Team, false);
        }

        private void SpawnNpc(Team team)
        {
            var pos = team == Team.Alpha
                ? alphaSpawnPoints[_alphaSpawnIndex++].position
                : bravoSpawnPoints[_bravoSpawnIndex++].position;

            var npc = Instantiate(networkObject, pos, Quaternion.identity);
            PlayerTransforms.Add(npc.transform);

            var initializer = npc.GetComponent<CharacterInitializer>();
            var npcData = ClientData.GetClientDataForNpc(team);
            initializer.SetCharacterParams(npcData);

            npc.Spawn();

            scoreManager.RegistCharacter(npcData.UUID, npcData.Team, true);

            TeamAlphaCount += team == Team.Alpha ? 1 : 0;
            TeamBravoCount += team == Team.Bravo ? 1 : 0;
        }
    }
}