using System;
using Unity.Netcode;

namespace RicoShot.Core.Interface
{
    public interface INetworkController
    {
        // Changed系はクライアントの接続時にも呼ばれる 
        // あらゆるクライアントデータの変更時に呼び出される
        public event Action OnClientDatasChanged;
        // クライアントがチームを変更したときに呼び出される
        public event Action OnTeamChanged;
        // クライアントのReady状態が変更されたときに呼び出される
        public event Action OnReadyStatusChanged;
        // すべてのクライアントがReady状態になったときに呼び出される
        public event Action OnAllClientsReady;
        // すべてのクライアントがReady状態のときにReadyが解除されると呼び出される
        public event Action OnAllClientsReadyCancelled;

        public NetworkList<ClientData> ClientDatas { get; }
        public NetworkVariable<bool> AllClientsReady { get; }

        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default);
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default);
        public void StartPlayRpc();
    }
}
