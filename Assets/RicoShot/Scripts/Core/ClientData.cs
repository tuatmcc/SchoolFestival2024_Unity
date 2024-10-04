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

        public ClientData(FixedString64Bytes UUID, ulong ClientID)
        {
            this.UUID = UUID;
            this.ClientID = ClientID;
            this.Team = Team.Alpha;
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
        }

        public override string ToString()
        {
            return $"ClientData -> UUID: {UUID}, ClientID:{ClientID}, Team: {Team}";
        }
    }
}