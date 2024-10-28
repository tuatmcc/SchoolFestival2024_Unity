using Chibi;
using Chibi.ChibiComponents;
using NaughtyAttributes;
using RicoShot.Core;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace RicoShot.Play.Tests
{
    public class PlaySceneTester : MonoBehaviour, IPlaySceneTester
    {
        public bool IsTest { get => testPlayScene; private set => testPlayScene = value; }
        public CharacterParams CharacterParams { get; private set; }

        [SerializeField] private bool testPlayScene = false;

        [EnableIf("testPlayScene")]
        [SerializeField]
        [Range(0, CharacterSettingsController.MAX_CHIBI_INDEX)]
        private int debugChibiIndex;

        [EnableIf("testPlayScene")]
        [SerializeField]
        private string debugHairColor = "#000000";

        [EnableIf("testPlayScene")]
        [SerializeField]
        [Range(0, ChibiCostumeColorSettings.MAX_COSTUME_VARIANT_INDEX)]
        private int debugCostumeVariant;

        [EnableIf("testPlayScene")]
        [SerializeField]
        [Range(0, ChibiAccessorySettings.MAX_ACCESSORY_INDEX)]
        private int debugAccessory;

        private void Awake()
        {
            CharacterParams = new CharacterParams(debugChibiIndex, debugHairColor, debugCostumeVariant, debugAccessory);
        }
    }
}
