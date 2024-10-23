using UnityEngine;

namespace Chibi
{
    [RequireComponent(typeof(ChibiHairColorController))]
    [RequireComponent(typeof(ChibiCostumeColorController))]
    [RequireComponent(typeof(ChibiAccessoryController))]
    public class ChibiSettingsController : MonoBehaviour
    {
        private ChibiAccessoryController _accessoryController;
        private ChibiCostumeColorController _costumeColorController;
        private ChibiHairColorController _hairColorController;

        public string hairColor
        {
            get => ColorUtility.ToHtmlStringRGB(_hairColorController.hairColor);
            set
            {
                if (!ColorUtility.TryParseHtmlString(value, out var color))
                {
                    Debug.LogError("Invalid color string");
                    return;
                }

                _hairColorController.hairColor = color;
            }
        }

        public int costumeVariant
        {
            get => _costumeColorController.costumeVariantIndex;
            set => _costumeColorController.costumeVariantIndex = value;
        }

        public int accessory
        {
            get => _accessoryController.accessoryIndex;
            set => _accessoryController.accessoryIndex = value;
        }

        private void Awake()
        {
            _hairColorController = GetComponent<ChibiHairColorController>();
            _costumeColorController = GetComponent<ChibiCostumeColorController>();
            _accessoryController = GetComponent<ChibiAccessoryController>();
        }
    }
}