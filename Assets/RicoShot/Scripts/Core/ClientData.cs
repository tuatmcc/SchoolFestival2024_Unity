using System;
using Unity.Collections;
using Unity.Netcode;

namespace RicoShot.Core
{
    public struct ClientData : INetworkSerializable, IEquatable<ClientData>
    {
        public FixedString64Bytes UUID;
        public ulong ClientID;
        public Team Team;
        public bool IsReady;

        public ClientData(FixedString64Bytes UUID, ulong ClientID)
        {
            this.UUID = UUID;
            this.ClientID = ClientID;
            this.Team = Team.Alpha;
            this.IsReady = false;
        }

        public void UpdateTeam(Team team)
        {
            this.Team = team;
        }

        public void UpdateReadyStatus(bool isReady)
        {
            this.IsReady = isReady;
        }

        public bool Equals(ClientData other)
        {
            throw new NotImplementedException();
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref UUID);
            serializer.SerializeValue(ref ClientID);
            serializer.SerializeValue(ref Team);
            serializer.SerializeValue(ref IsReady);
        }

        public override string ToString()
        {
            return $"ClientData -> UUID: {UUID}, ClientID:{ClientID}, Team: {Team}, IsReady: {IsReady}";
        }
    }
}