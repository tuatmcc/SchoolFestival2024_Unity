using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    public class CharacterSettingsController : MonoBehaviour
    {
        [SerializeField] private ChibiSettingsController[] chibiSettingsController;

        [SerializeField] private bool debugMode;

        [EnableIf("debugMode")] [SerializeField]
        private int debugChibiIndex;

        private int _chibiIndex;

        public int activeChibiIndex
        {
            get => _chibiIndex;
            set
            {
                if (value is < 0 or >= 5 || debugChibiIndex is < 0 or >= 5)
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
            set => chibiSettingsController[activeChibiIndex].hairColor = value;
        }

        public int costumeVariant
        {
            get => chibiSettingsController[activeChibiIndex].costumeVariant;
            set => chibiSettingsController[activeChibiIndex].costumeVariant = value;
        }

        public int accessory
        {
            get => chibiSettingsController[activeChibiIndex].accessory;
            set => chibiSettingsController[activeChibiIndex].accessory = value;
        }

        private void Awake()
        {
            activeChibiIndex = _chibiIndex;
        }
    }
}