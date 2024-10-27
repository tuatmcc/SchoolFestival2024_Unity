using RicoShot.Core;
using RicoShot.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace RicoShot.Core
{
    /// <summary>
    /// マッチング時に使用されるクライアントデータ保存用クラス
    /// </summary>
    [Serializable]
    public class ClientData : INetworkSerializable, IEquatable<ClientData>, IDataChangedNotifiable
    {
        // NetworkClassListで使うために値が変更されたことを通知するイベントを実装
        public event Action OnDataChanged;

        public FixedString64Bytes UUID { get => uuid; private set => uuid = value; }
        public ulong ClientID { get => clientID; private set => clientID = value; }
        public FixedString64Bytes Name { get => name; private set => name = value; }
        public Team Team
        {
            get => team;
            set
            {
                team = value;
                OnDataChanged?.Invoke();
            }
        }
        public bool IsReady
        {
            get => isReady;
            set
            {
                isReady = value;
                OnDataChanged?.Invoke();
            }
        }
        public CharacterParams CharacterParams { get => characterParams; private set => characterParams = value; }
        public bool IsNpc { get; set; }

        private FixedString64Bytes uuid = default;
        private ulong clientID = default;
        private FixedString64Bytes name = default;
        private Team team = default;
        private bool isReady = default;
        private CharacterParams characterParams = new();
        private bool isNpc = default;

        public ClientData()
        {

        }

        public ClientData(FixedString64Bytes UUID, ulong ClientID, CharacterParams CharacterParams)
        {
            this.UUID = UUID;
            this.ClientID = ClientID;
            this.CharacterParams = CharacterParams;
            this.IsNpc = false;
        }

        public ClientData(Team Team, CharacterParams CharacterParams, ulong ClientID = NetworkManager.ServerClientId, bool IsNpc = true)
        {
            this.ClientID = ClientID;
            this.Team = Team;
            this.CharacterParams = CharacterParams;
            this.IsNpc = IsNpc;
        }

        public override string ToString()
        {
            return $"ClientData -> UUID: {UUID}, ClientID:{ClientID}, Name: {Name}, Team: {Team}, IsReady: {IsReady}";
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref uuid);
            serializer.SerializeValue(ref clientID);
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref team);
            serializer.SerializeValue(ref isReady);
            serializer.SerializeValue(ref isNpc);
            CharacterParams.NetworkSerialize(serializer);
        }

        public bool Equals(ClientData other)
        {
            return this.ClientID == other.ClientID;
        }
    }
}
