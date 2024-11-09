using Chibi;
using Chibi.ChibiComponents;
using RicoShot.Core;
using RicoShot.Utils;
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
    public class CharacterParams : INetworkSerializable, IDataChangedNotifiable
    {
        public event Action OnDataChanged;

        public int ChibiIndex
        {
            get => chibiIndex;
            set
            {
                chibiIndex = value;
                OnDataChanged?.Invoke();
            }
        }
        public FixedString32Bytes HairColor
        {
            get => hairColor;
            set
            {
                hairColor = value;
                OnDataChanged?.Invoke();
            }
        }
        public int CostumeVariant { 
            get => costumeVariant; 
            set 
            {
                costumeVariant = value;
                OnDataChanged?.Invoke();
            } 
        }
        public int Accessory { 
            get => accessory; 
            set 
            {
                accessory = value;
                OnDataChanged?.Invoke();
            } 
        }

        public CharacterParams()
        {
            ChibiIndex = 0;
            HairColor = "#000000";
            CostumeVariant = 0;
            Accessory = 0;
        }
    
        public CharacterParams(int ChibiIndex, FixedString32Bytes HairColor,  int CostumeVariant, int Accessory)
        {
            this.ChibiIndex = ChibiIndex;
            this.HairColor = HairColor;
            this.CostumeVariant = CostumeVariant;
            this.Accessory = Accessory;
        }

        public static CharacterParams GetRandomCharacterParams()
        {
            var characterParams = new CharacterParams
            {
                ChibiIndex = UnityEngine.Random.Range(0, CharacterSettingsController.MAX_CHIBI_INDEX + 1),
                CostumeVariant = UnityEngine.Random.Range(0, ChibiCostumeColorSettings.MAX_COSTUME_VARIANT_INDEX + 1),
                HairColor = HairColorPresets[UnityEngine.Random.Range(0, HairColorPresets.Length)],
                Accessory = UnityEngine.Random.Range(0, ChibiAccessorySettings.MAX_ACCESSORY_INDEX + 1)
            };
            return characterParams;
        }

        [SerializeField] private int chibiIndex = 0;
        [SerializeField] private FixedString32Bytes hairColor = "#000000";
        [SerializeField] private int costumeVariant = 0;
        [SerializeField] private int accessory = 0;

        private static readonly string[] HairColorPresets = new string[] {
            "#333333",
            "#ededed",
            "#582a22",
            "#d53b10",
            "#c56a20",
            "#a1d6f8",
            "#f5cd3d",
            "#257901",
            "#f485bb",
            // "#57a06d",
            "#345e95",
        };

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref chibiIndex);
            serializer.SerializeValue(ref hairColor);
            serializer.SerializeValue(ref costumeVariant);
            serializer.SerializeValue(ref accessory);
        }

        public override string ToString()
        {
            return $"CharacterParams: ChibiIndex => {ChibiIndex}, HairColor => {HairColor}, CostumeVariant => {CostumeVariant}, Accessory => {Accessory}";
        }
    }
}
