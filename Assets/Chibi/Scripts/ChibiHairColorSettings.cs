using Chibi.ChibiColorVariations;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     髪を任意の色に設定するクラス。また、Inspector上で色を変更してデバッグするための機能を提供します
    /// </summary>
    [RequireComponent(typeof(ChibiCostumeColors))]
    public class ChibiHairColorSettings : MonoBehaviour
    {
        [SerializeField] private Material hairMaterial;
        [SerializeField] private SkinnedMeshRenderer[] hairMesh;
        [SerializeField] private Color hair;

        private Material _hairMaterialInstance;

        public Color hairColor
        {
            get => hair;
            set
            {
                hair = value;
                // this check is necessary to prevent the material from being created multiple times
                if (_hairMaterialInstance == null)
                {
                    _hairMaterialInstance = new Material(hairMaterial);
                    foreach (var x in hairMesh) x.material = _hairMaterialInstance;
                }

                _hairMaterialInstance.color = hair;
            }
        }

        private void Awake()
        {
            if (_hairMaterialInstance == null)
            {
                _hairMaterialInstance = new Material(hairMaterial);
                foreach (var x in hairMesh) x.material = _hairMaterialInstance;
            }
        }

        private void OnDestroy()
        {
            Destroy(_hairMaterialInstance);
        }

        [Button]
        private void ApplyColorImmediately()
        {
            hairColor = hair;
        }
    }
}