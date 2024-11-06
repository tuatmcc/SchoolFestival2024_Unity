using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Play.Tests
{
    public class DummyNetworkController : INetworkController
    {
        public NetworkClassList<ClientData> ClientDataList => throw new NotImplementedException();

        public NetworkVariable<bool> AllClientsReady => throw new NotImplementedException();

        public INetworkScoreManager ScoreManager { get => throw new NotImplementedException(); set { } }

        public event Action<bool> OnAllClientsReadyChanged;
        public event Action OnServerConnectionCompleted;

        public void StartPlayRpc()
        {
            throw new NotImplementedException();
        }

        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default)
        {
            throw new NotImplementedException();
        }

        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default)
        {
            throw new NotImplementedException();
        }
    }
}
