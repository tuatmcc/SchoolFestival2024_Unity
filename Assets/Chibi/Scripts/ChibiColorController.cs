using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     髪を任意の色に設定するクラス
    /// </summary>
    public class ChibiColorController : MonoBehaviour
    {
        [Foldout("Materials")] [SerializeField]
        private Material hairMaterial;

        [Foldout("Materials")] [SerializeField]
        private Material clothesMaterial;

        [SerializeField] private Color hair;
        [SerializeField] private Color clothes;

        [NaughtyAttributes.Button]
        private void ApplyColorImmediately()
        {
            SetColors(hair, clothes);
        }

        public void SetColors(Color hairColor, Color clothesColor)
        {
            hairMaterial.color = hairColor;
            clothesMaterial.color = clothesColor;
        }
    }
}