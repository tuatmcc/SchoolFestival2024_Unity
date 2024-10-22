using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    /// <summary>
    ///     顔アクセサリを制御するクラス
    /// </summary>
    public class ChibiAccessoryController : MonoBehaviour
    {
        private const int AccessoryCount = 4;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer glasses;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer goggles;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer mask;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer eyePatch;

        [Range(0, AccessoryCount)] [SerializeField]
        private int accessory;

        public int accessoryIndex
        {
            get => accessory;
            set
            {
                if (value is < 0 or > AccessoryCount)
                {
                    Debug.LogError("Invalid accessory index");
                    return;
                }

                accessory = value;

                // if index is 0, disable all accessories
                glasses.gameObject.SetActive(value == 1);
                goggles.gameObject.SetActive(value == 2);
                mask.gameObject.SetActive(value == 3);
                eyePatch.gameObject.SetActive(value == 4);
            }
        }

        [Button]
        private void ApplyAccessoryImmediately()
        {
            accessoryIndex = accessory;
        }
    }
}