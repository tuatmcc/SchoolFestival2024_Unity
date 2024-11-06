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
    }
}