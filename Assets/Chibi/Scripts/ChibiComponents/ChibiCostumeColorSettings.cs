using Chibi.ChibiComponents.ChibiColorVariations;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi.ChibiComponents
{
    /// <summary>
    ///     服の色をキャラごとのパターンからindexで選択するクラス。また、Inspector上でindexを変更してデバッグするための機能を提供します
    /// </summary>
    [RequireComponent(typeof(ChibiCostumeColors))]
    public class ChibiCostumeColorSettings : MonoBehaviour
    {
        public const int MAX_COSTUME_VARIANT_INDEX = 2;

        [SerializeField] private SkinnedMeshRenderer[] clothesMeshes;
        [SerializeField] private Material clothesMaterial;

        [Range(0, MAX_COSTUME_VARIANT_INDEX)] [SerializeField]
        private int costumeVariant;

        [SerializeField] private ChibiCostumeColors chibiCostumeColors;

        private Material _clothesMaterialInstance;

        public int costumeVariantIndex
        {
            get => costumeVariant;
            set
            {
                if (value is < 0 or > MAX_COSTUME_VARIANT_INDEX)
                {
                    Debug.LogError("Invalid costume variation index");
                    return;
                }

                if (_clothesMaterialInstance == null)
                {
                    _clothesMaterialInstance = new Material(clothesMaterial);
                    foreach (var x in clothesMeshes)
                        x.material = _clothesMaterialInstance;
                }

                costumeVariant = value;
                _clothesMaterialInstance.color = chibiCostumeColors.GetColor(costumeVariant);
            }
        }

        private void Awake()
        {
            if (_clothesMaterialInstance == null)
            {
                _clothesMaterialInstance = new Material(clothesMaterial);
                foreach (var x in clothesMeshes)
                    x.material = _clothesMaterialInstance;
            }
        }

        private void OnDestroy()
        {
            Destroy(_clothesMaterialInstance);
        }

        [Button]
        private void ApplyColorImmediately()
        {
            costumeVariantIndex = costumeVariant;
        }
    }
}