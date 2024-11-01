using Chibi;
using Chibi.ChibiComponents;
using NaughtyAttributes;
using RicoShot.Core;
using RicoShot.Result.Interface;
using UnityEngine;
using Zenject;

namespace RicoShot.Result
{
    public class ResultSceneTester : MonoBehaviour, IResultSceneTester
    {
        [SerializeField] bool isTest;

        [EnableIf("isTest")] [SerializeField] [Range(0, ResultCameraWorkSwitcher.CameraWorkMaxIndex)]
        int cameraWorkIndex;

        [EnableIf("isTest")] [SerializeField] [Range(0, CharacterSettingsController.MAX_CHIBI_INDEX)]
        private int chibiIndex;

        [EnableIf("isTest")] [SerializeField] private string hairColor = "#000000";

        [EnableIf("isTest")] [SerializeField] [Range(0, ChibiCostumeColorSettings.MAX_COSTUME_VARIANT_INDEX)]
        private int costumeVariant;

        [EnableIf("isTest")] [SerializeField] [Range(0, ChibiAccessorySettings.MAX_ACCESSORY_INDEX)]
        private int accessoryIndex;

        private IResultSceneTester _resultSceneTesterImplementation;

        public bool TestEnabled => isTest;

        public CharacterParams CharacterParams { get; private set; }
        public int CameraWorkIndex { get; private set; }

        private void Awake()
        {
            if (isTest)
            {
                CharacterParams = new CharacterParams(chibiIndex, hairColor, costumeVariant, accessoryIndex);
            }
        }
    }
}