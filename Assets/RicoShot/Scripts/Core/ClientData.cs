using RicoShot.Core;
using RicoShot.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace RicoShot.Core
{
    /// <summary>
    /// マッチング時に使用されるクライアントデータ保存用クラス
    /// リファクタリング予定(優先度:低)
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

        [SerializeField] private FixedString64Bytes uuid = default;
        [SerializeField] private ulong clientID = default;
        [SerializeField] private FixedString64Bytes name = default;
        [SerializeField] private Team team = default;
        [SerializeField] private bool isReady = default;
        private CharacterParams characterParams = new CharacterParams();

        public ClientData()
        {

        }

        public ClientData(FixedString64Bytes UUID, ulong ClientID, CharacterParams CharacterParams)
        {
            this.UUID = UUID;
            this.ClientID = ClientID;
            //this.CharacterParams = CharacterParams;
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
            // NetworkValiableをネストできないので力ずくでSerialize(半ば強引であるが…)
            var fields = characterParams.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                foreach(var field in fields)
                {
                    var method = typeof(FastBufferReader).GetMethod("ReadValueSafe", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { field.FieldType }, null);
                    var genericMethod = method.MakeGenericMethod(field.FieldType);
                    var parameters = new object[] { null };
                    genericMethod.Invoke(reader, parameters);
                    field.SetValue(CharacterParams, parameters[0]);
                }
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                foreach(var field in fields)
                {
                    var method = typeof(FastBufferWriter).GetMethod("WriteValueSafe", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { field.FieldType }, null);
                    var genericMethod = method.MakeGenericMethod(field.FieldType);
                    genericMethod.Invoke(writer, new object[] { field.GetValue(CharacterParams) });
                }
            }
        }

        public bool Equals(ClientData other)
        {
            return this.ClientID == other.ClientID;
        }
    }
}
