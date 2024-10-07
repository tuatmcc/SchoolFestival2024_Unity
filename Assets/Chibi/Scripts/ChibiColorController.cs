using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     髪を任意の色に設定するクラス
    /// </summary>
    public class ChibiColorController : MonoBehaviour
    {
        [SerializeField] private Color hair;
        [SerializeField] private Color clothes;
        [SerializeField] private Material hairMaterial;
        [SerializeField] private Material clothesMaterial;

        private void OnEnable()
        {
            hairMaterial.color = hair;
            clothesMaterial.color = clothes;
        }

        public void SetColors(Color hairColor, Color clothesColor)
        {
            hairMaterial.color = hairColor;
            clothesMaterial.color = clothesColor;
        }
    }
}