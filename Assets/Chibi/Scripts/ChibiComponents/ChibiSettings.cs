using NaughtyAttributes;
using UnityEngine;

namespace Chibi.ChibiComponents
{
    [RequireComponent(typeof(ChibiHairColorSettings))]
    [RequireComponent(typeof(ChibiCostumeColorSettings))]
    [RequireComponent(typeof(ChibiAccessorySettings))]
    public class ChibiSettings : MonoBehaviour
    {
        [SerializeField] private ChibiAccessorySettings accessorySettings;

        [SerializeField] private ChibiCostumeColorSettings costumeColorSettings;

        [SerializeField] private ChibiHairColorSettings hairColorSettings;

        public string hairColor
        {
            get => ColorUtility.ToHtmlStringRGB(hairColorSettings.hairColor);
            set
            {
                if (!ColorUtility.TryParseHtmlString(value, out var color))
                {
                    Debug.LogError("Invalid color string");
                    return;
                }

                hairColorSettings.hairColor = color;
            }
        }

        public int costumeVariant
        {
            get => costumeColorSettings.costumeVariantIndex;
            set => costumeColorSettings.costumeVariantIndex = value;
        }

        public int accessory
        {
            get => accessorySettings.accessoryIndex;
            set => accessorySettings.accessoryIndex = value;
        }

        [Button]
        private void AttachComponents()
        {
            hairColorSettings = GetComponent<ChibiHairColorSettings>();
            costumeColorSettings = GetComponent<ChibiCostumeColorSettings>();
            accessorySettings = GetComponent<ChibiAccessorySettings>();
        }
    }
}