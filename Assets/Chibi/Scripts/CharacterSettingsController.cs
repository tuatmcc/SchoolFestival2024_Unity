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
        [SerializeField] private ChibiSettingsController[] chibiSettingsController;

        // debugMode の条件判定は高頻度で呼ばれないのでパフォーマンスには影響しないはず...
        [SerializeField] private bool debugMode;

        [EnableIf("debugMode")] [SerializeField] [Range(0, MAX_CHIBI_INDEX)]
        private int debugChibiIndex;

        [EnableIf("debugMode")] [SerializeField]
        private string debugHairColor = "#000000";

        [EnableIf("debugMode")] [SerializeField] [Range(0, ChibiCostumeColorController.MAX_COSTUME_VARIANT_INDEX)]
        private int debugCostumeVariant;

        [EnableIf("debugMode")] [SerializeField] [Range(0, ChibiAccessoryController.MAX_ACCESSORY_INDEX)]
        private int debugAccessory;

        private int _chibiIndex;

        public int activeChibiIndex
        {
            get => _chibiIndex;
            set
            {
                if (value is < 0 or >= MAX_CHIBI_INDEX || debugChibiIndex is < 0 or >= MAX_CHIBI_INDEX)
                {
                    Debug.LogError("Invalid chibi index");
                    return;
                }

                _chibiIndex = debugMode ? debugChibiIndex : value;
                foreach (var chibiSettings in chibiSettingsController) chibiSettings.gameObject.SetActive(false);
                chibiSettingsController[activeChibiIndex].gameObject.SetActive(true);
            }
        }

        public string hairColor
        {
            get => chibiSettingsController[activeChibiIndex].hairColor;
            set
            {
                if (debugMode)
                    chibiSettingsController[debugChibiIndex].hairColor = debugHairColor;
                else
                    chibiSettingsController[activeChibiIndex].hairColor = value;
            }
        }

        public int costumeVariant
        {
            get => chibiSettingsController[activeChibiIndex].costumeVariant;
            set
            {
                if (debugMode)
                    chibiSettingsController[debugChibiIndex].costumeVariant = debugCostumeVariant;
                else
                    chibiSettingsController[activeChibiIndex].costumeVariant = value;
            }
        }

        public int accessory
        {
            get => chibiSettingsController[activeChibiIndex].accessory;
            set
            {
                if (debugMode)
                    chibiSettingsController[debugChibiIndex].accessory = debugAccessory;
                else
                    chibiSettingsController[activeChibiIndex].accessory = value;
            }
        }

        private void Awake()
        {
            // validate chibi index
            if (MAX_CHIBI_INDEX != chibiSettingsController.Length - 1)
            {
                Debug.LogError("Max chibi index does not match the number of chibi settings controllers");
                return;
            }

            activeChibiIndex = _chibiIndex;
            if (debugMode)
            {
                hairColor = debugHairColor;
                costumeVariant = debugCostumeVariant;
                accessory = debugAccessory;
            }
        }
    }
}