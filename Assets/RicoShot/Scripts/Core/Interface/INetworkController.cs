using System;
using Unity.Netcode;

namespace RicoShot.Core.Interface
{
    public interface INetworkController
    {
        // すべてのクライアントがReady状態になった・解除されたときに呼び出される
        public event Action<bool> OnAllClientsReadyChanged;

        public NetworkClassList<ClientData> ClientDatas { get; }
        public NetworkVariable<bool> AllClientsReady { get; }

        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default);
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default);
        public void StartPlayRpc();
    }
}
