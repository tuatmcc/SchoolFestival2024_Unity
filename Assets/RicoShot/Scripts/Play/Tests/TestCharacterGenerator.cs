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
    public class TestCharacterGenerator : MonoBehaviour
    {
        [SerializeField] private NetworkObject networkObject;

        private static int TeamAlphaNum = 5;
        private static int TeamBravoNum = 5;
        private int TeamAlphaCount = 0;
        private int TeamBravoCount = 0;

        [Inject] private readonly INetworkController networkController;
        [Inject] private readonly INetworkScoreManager scoreManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

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

            foreach (var clientData in networkController.ClientDataList)
            {
                SpawnPlayer(clientData);
            }

            while (TeamAlphaCount < TeamAlphaNum)
            {
                SpawnNpc(Team.Alpha);
            }

            while (TeamBravoCount < TeamBravoNum)
            {
                SpawnNpc(Team.Bravo);
            }
        }

        private void SpawnPlayer(ClientData clientData)
        {
            var pos = new Vector3 (
                (clientData.Team == Team.Alpha ? TeamAlphaCount : TeamBravoCount) * 3,
                0,
                (clientData.Team == Team.Alpha ? -2 : 2));

            var player = Instantiate(networkObject, pos, Quaternion.identity);

            TeamAlphaCount += clientData.Team == Team.Alpha ? 1 : 0;
            TeamBravoCount += clientData.Team == Team.Bravo ? 1 : 0;

            player.DontDestroyWithOwner = true;

            var initializer = player.GetComponent<CharacterInitializer>();
            initializer.SetCharacterParams(clientData);

            player.SpawnAsPlayerObject(clientData.ClientID);
            Debug.Log($"Created character: { clientData.ClientID }");

            scoreManager.RegistCharacter(clientData.UUID, clientData.Team, false);
        }

        private void SpawnNpc(Team team)
        {
            var pos = new Vector3(
                (team == Team.Alpha ? TeamAlphaCount : TeamBravoCount) * 3,
                0,
                (team == Team.Alpha ? -2 : 2));

            var npc = Instantiate(networkObject, pos, Quaternion.identity);

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
