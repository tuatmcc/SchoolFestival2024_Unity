using UnityEngine;
using UnityEngine.Serialization;

namespace Chibi
{
    [RequireComponent(typeof(ChibiHairColorSettings))]
    [RequireComponent(typeof(ChibiCostumeColorSettings))]
    [RequireComponent(typeof(ChibiAccessorySettings))]
    public class ChibiSettings : MonoBehaviour
    {
        [FormerlySerializedAs("accessoryController")] [SerializeField]
        private ChibiAccessorySettings accessorySettings;

        [FormerlySerializedAs("costumeColorController")] [SerializeField]
        private ChibiCostumeColorSettings costumeColorSettings;

        [FormerlySerializedAs("hairColorController")] [SerializeField]
        private ChibiHairColorSettings hairColorSettings;

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
    }
}