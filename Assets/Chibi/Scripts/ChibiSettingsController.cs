using UnityEngine;

namespace Chibi
{
    [RequireComponent(typeof(ChibiHairColorController))]
    [RequireComponent(typeof(ChibiCostumeColorController))]
    [RequireComponent(typeof(ChibiAccessoryController))]
    public class ChibiSettingsController : MonoBehaviour
    {
        private ChibiAccessoryController accessoryController;
        private ChibiCostumeColorController costumeColorController;
        private ChibiHairColorController hairColorController;

        public string hairColor
        {
            get => ColorUtility.ToHtmlStringRGB(hairColorController.hairColor);
            set
            {
                if (!ColorUtility.TryParseHtmlString(value, out var color))
                {
                    Debug.LogError("Invalid color string");
                    return;
                }

                hairColorController.hairColor = color;
            }
        }

        public int costumeVariant
        {
            get => costumeColorController.costumeVariantIndex;
            set => costumeColorController.costumeVariantIndex = value;
        }

        public int accessory
        {
            get => accessoryController.accessoryIndex;
            set => accessoryController.accessoryIndex = value;
        }

        private void Awake()
        {
            hairColorController = GetComponent<ChibiHairColorController>();
            costumeColorController = GetComponent<ChibiCostumeColorController>();
            accessoryController = GetComponent<ChibiAccessoryController>();
        }
    }
}