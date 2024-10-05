using RicoShot.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Core
{
    [Serializable]
    public class ClientData : INetworkSerializable, IEquatable<ClientData>
    {
        public FixedString64Bytes UUID = default;
        public ulong ClientID = default;
        public FixedString64Bytes Name = default;
        public Team Team = default;
        public bool IsReady = default;

        public ClientData()
        {

        }

        public ClientData(FixedString64Bytes UUID, ulong ClientID)
        {
            this.UUID = UUID;
            this.ClientID = ClientID;
        }

        public override string ToString()
        {
            return $"ClientData -> UUID: {UUID}, ClientID:{ClientID}, Name: {Name}, Team: {Team}, IsReady: {IsReady}";
        }

        public void SetTeam(Team team)
        {
            Team = team;
        }

        public void SetReadyStatus(bool isReady)
        {
            IsReady = isReady;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref UUID);
            serializer.SerializeValue(ref ClientID);
            serializer.SerializeValue(ref Name);
            serializer.SerializeValue(ref Team);
            serializer.SerializeValue(ref IsReady);
        }

        public bool Equals(ClientData other)
        {
            return this.UUID == other.UUID;
        }
    }
}
