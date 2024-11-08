using Chibi;
using RicoShot.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RicoShot.Matching.UI
{
    public class MatchingPlayerPreviewFrame : MonoBehaviour
    {
        [SerializeField] private Image readyImage;
        [SerializeField] private Image waitingImage;
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private TMP_Text displayNameShadow;
        [SerializeField] private RawImage image;
        [SerializeField] private CharacterSettingsController characterSettingsController;

        private void Awake()
        {
            SetReady(false);
        }

        public void SetPlayer(string playerDisplayName)
        {
            displayName.text = playerDisplayName;
            displayNameShadow.text = playerDisplayName;
        }

        public void SetReady(bool ready)
        {
            readyImage.gameObject.SetActive(ready);
            waitingImage.gameObject.SetActive(!ready);
        }

        public void ApplyCharacterParams(CharacterParams characterParams)
        {
            characterSettingsController.activeChibiIndex = characterParams.ChibiIndex;
            characterSettingsController.hairColor = characterParams.HairColor.ToString();
            characterSettingsController.costumeVariant = characterParams.CostumeVariant;
            characterSettingsController.accessory = characterParams.Accessory;
        }
    }
}