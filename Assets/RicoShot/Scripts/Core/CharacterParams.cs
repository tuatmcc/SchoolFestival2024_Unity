using RicoShot.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Core
{
    /// <summary>
    /// キャラクターに関するパラメーターを保持するクラス
    /// </summary>
    [Serializable]
    public class CharacterParams : INetworkSerializable
    {
        public int ChibiIndex { get => chibiIndex; private set => chibiIndex = value; }
        public FixedString32Bytes HairColor { get => hairColor; private set => hairColor = value; }
        public int CostumeVariant { get => costumeVariant; private set => costumeVariant = value; }
        public int Accessory { get => accessory; private set => accessory = value; }

        public CharacterParams()
        {
            ChibiIndex = 0;
            HairColor = "#000000";
            CostumeVariant = 0;
            Accessory = 0;
        }
    
        [SerializeField] private int chibiIndex;
        [SerializeField] private FixedString32Bytes hairColor;
        [SerializeField] private int costumeVariant;
        [SerializeField] private int accessory;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref chibiIndex);
            serializer.SerializeValue(ref hairColor);
            serializer.SerializeValue(ref costumeVariant);
            serializer.SerializeValue(ref accessory);
        }
    }
}
