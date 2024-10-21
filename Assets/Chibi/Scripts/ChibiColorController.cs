using Chibi.ChibiColorVariations;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     髪を任意の色に設定するクラス
    /// </summary>
    [RequireComponent(typeof(ChibiColor))]
    public class ChibiColorController : MonoBehaviour
    {
        [Foldout("Materials")] [SerializeField]
        private Material hairMaterial;

        [Foldout("Materials")] [SerializeField]
        private Material clothesMaterial;

        [SerializeField] private Color hair;
        [SerializeField] private Color clothes;

        [Range(0, 2)] [SerializeField] private int colorVariationIndex;

        [SerializeField] private ChibiColor chibiColor;

        [Button]
        private void ApplyColorImmediately()
        {
            SetColors(hair, colorVariationIndex);
        }

        public void SetColors(Color hairColor, int variationIndex)
        {
            hairMaterial.color = hairColor;
            clothesMaterial.color = chibiColor.GetColor(variationIndex);
        }
    }
}