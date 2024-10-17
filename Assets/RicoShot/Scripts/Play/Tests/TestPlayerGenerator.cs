using RicoShot.Core;
using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestPlayerGenerator : MonoBehaviour
    {
        [SerializeField] private NetworkObject networkObject;

        private static int TeamAlphaNum = 5;
        private static int TeamBravoNum = 5;
        private int TeamAlphaCount = 0;
        private int TeamBravoCount = 0;

        [Inject] INetworkController networkController;

        // クライアントのプレイヤーを生成
        private void Start()
        {
            // サーバー側のみの処理
            if (!NetworkManager.Singleton.IsServer) return;

            foreach (var clientData in networkController.ClientDatas)
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

            player.SpawnAsPlayerObject(clientData.ClientID);
            Debug.Log($"Created { clientData}");
        }

        private void SpawnNpc(Team team)
        {
            var pos = new Vector3(
                (team == Team.Alpha ? TeamAlphaCount : TeamBravoCount) * 3,
                0,
                (team == Team.Alpha ? -2 : 2));

            var player = Instantiate(networkObject, pos, Quaternion.identity);

            TeamAlphaCount += team == Team.Alpha ? 1 : 0;
            TeamBravoCount += team == Team.Bravo ? 1 : 0;

            player.Spawn();
        }
    }
}
