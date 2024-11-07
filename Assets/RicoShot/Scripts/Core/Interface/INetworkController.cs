using RicoShot.Play;
using RicoShot.Play.Interface;
using System;
using Unity.Netcode;

namespace RicoShot.Core.Interface
{
    public interface INetworkController
    {
        // すべてのクライアントがReady状態になった・解除されたときに呼び出される
        public event Action<bool> OnAllClientsReadyChanged;

        // (クライアント)サーバーへの接続が完了した時に呼び出される
        public event Action OnServerConnectionCompleted;

        public NetworkClassList<ClientData> ClientDataList { get; }
        public NetworkVariable<bool> AllClientsReady { get; }
        public INetworkScoreManager ScoreManager { get; set; }

        public void UpdateTeamRpc(Team team, RpcParams rpcParams = default);
        public void UpdateReadyStatusRpc(bool isReady, RpcParams rpcParams = default);
        public void StartPlayRpc();
    }
}