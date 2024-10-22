using Chibi.ChibiColorVariations;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     髪を任意の色に設定するクラス。また、Inspector上で色を変更してデバッグするための機能を提供します
    /// </summary>
    [RequireComponent(typeof(ChibiCostumeColors))]
    public class ChibiHairColorController : MonoBehaviour
    {
        [Foldout("Materials")] [SerializeField]
        private Material hairMaterial;

        [Foldout("Materials")] [SerializeField]
        private Material clothesMaterial;

        [SerializeField] private Color hair;
        [SerializeField] private ChibiCostumeColors chibiCostumeColors;

        public Color hairColor
        {
            get => hair;
            set
            {
                hair = value;
                hairMaterial.color = hair;
            }
        }


        [Button]
        private void ApplyColorImmediately()
        {
            hairColor = hair;
        }
    }
}