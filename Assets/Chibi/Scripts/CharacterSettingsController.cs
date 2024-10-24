using Chibi.ChibiComponents;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     キャラクターの設定を管理するクラス. チビの種類の選択、髪の色、服の色、アクセサリの選択を一括でできる。
    /// </summary>
    public class CharacterSettingsController : MonoBehaviour
    {
        public const int MAX_CHIBI_INDEX = 4;

        [SerializeField] private CharacterChibiSwitcher chibiSwitcher;

        // debugMode の条件判定は高頻度で呼ばれないのでパフォーマンスには影響しないはず...
        [SerializeField] private bool debugMode;

        [EnableIf("debugMode")] [SerializeField] [Range(0, MAX_CHIBI_INDEX)]
        private int debugChibiIndex;

        [EnableIf("debugMode")] [SerializeField]
        private string debugHairColor = "#000000";

        [EnableIf("debugMode")] [SerializeField] [Range(0, ChibiCostumeColorSettings.MAX_COSTUME_VARIANT_INDEX)]
        private int debugCostumeVariant;

        [EnableIf("debugMode")] [SerializeField] [Range(0, ChibiAccessorySettings.MAX_ACCESSORY_INDEX)]
        private int debugAccessory;

        public int activeChibiIndex
        {
            get => chibiSwitcher.currentChibiIndex;
            set
            {
                if (value is < 0 or > MAX_CHIBI_INDEX)
                {
                    Debug.LogError("Invalid chibi index");
                    return;
                }

                chibiSwitcher.currentChibiIndex = debugMode ? debugChibiIndex : value;
            }
        }

        public ChibiSettings activeChibiSettings =>
            chibiSwitcher.current.chibiSettings;

        public string hairColor
        {
            get => chibiSwitcher.current.chibiSettings.hairColor;
            set
            {
                if (debugMode)
                    chibiSwitcher.current.chibiSettings.hairColor = debugHairColor;
                else
                    chibiSwitcher.current.chibiSettings.hairColor = value;
            }
        }

        public int costumeVariant
        {
            get => chibiSwitcher.current.chibiSettings.costumeVariant;
            set
            {
                if (debugMode)
                    chibiSwitcher.current.chibiSettings.costumeVariant = debugCostumeVariant;
                else
                    chibiSwitcher.current.chibiSettings.costumeVariant = value;
            }
        }

        public int accessory
        {
            get => chibiSwitcher.current.chibiSettings.accessory;
            set
            {
                if (debugMode)
                    chibiSwitcher.current.chibiSettings.accessory = debugAccessory;
                else
                    chibiSwitcher.current.chibiSettings.accessory = value;
            }
        }

        public ChibiComponents.Chibi animator => chibiSwitcher.current;

        private void Awake()
        {
            if (debugMode)
            {
                activeChibiIndex = debugChibiIndex;
                hairColor = debugHairColor;
                costumeVariant = debugCostumeVariant;
                accessory = debugAccessory;
            }
        }

        private void OnValidate()
        {
            if (chibiSwitcher == null)
            {
                Debug.LogError("Chibi switcher is not set");
                return;
            }

            if (chibiSwitcher.chibis.Length != MAX_CHIBI_INDEX + 1)
                Debug.LogError("Number of chibis does not match the max chibi index");
        }
    }
}