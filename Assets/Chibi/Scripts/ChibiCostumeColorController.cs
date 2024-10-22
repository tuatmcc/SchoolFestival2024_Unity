using Chibi.ChibiColorVariations;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     服の色をキャラごとのパターンからindexで選択するクラス。また、Inspector上でindexを変更してデバッグするための機能を提供します
    /// </summary>
    [RequireComponent(typeof(ChibiCostumeColors))]
    public class ChibiCostumeColorController : MonoBehaviour
    {
        private const int MaxCostumeVariantIndex = 2;

        [Foldout("Materials")] [SerializeField]
        private Material clothesMaterial;

        [Range(0, MaxCostumeVariantIndex)] [SerializeField]
        private int costumeVariant;

        [SerializeField] private ChibiCostumeColors chibiCostumeColors;

        public int costumeVariantIndex
        {
            get => costumeVariant;
            set
            {
                if (value is < 0 or > MaxCostumeVariantIndex)
                {
                    Debug.LogError("Invalid costume variation index");
                    return;
                }

                costumeVariant = value;
                clothesMaterial.color = chibiCostumeColors.GetColor(costumeVariant);
            }
        }

        [Button]
        private void ApplyColorImmediately()
        {
            costumeVariantIndex = costumeVariant;
        }
    }
}