using System;
using NaughtyAttributes;
using UnityEngine;

namespace Chibi
{
    public class ChibiAccessoryController : MonoBehaviour
    {
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer glasses;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer goggles;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer mask;
        [Foldout("Meshes")] [SerializeField] private SkinnedMeshRenderer eyePatch;
        const int AccessoryCount = 4;

        [Range(0, AccessoryCount)] [SerializeField]
        private int accessoryIndex;

        [NaughtyAttributes.Button]
        private void ApplyAccessoryImmediately()
        {
            SetActiveAccessory(accessoryIndex);
        }


        public void SetActiveAccessory(int index)
        {
            if (index < 0 || index > AccessoryCount)
            {
                Debug.LogError("Invalid accessory index");
                return;
            }

            // if index is 0, disable all accessories
            glasses.gameObject.SetActive(index == 1);
            goggles.gameObject.SetActive(index == 2);
            mask.gameObject.SetActive(index == 3);
            eyePatch.gameObject.SetActive(index == 4);
        }
    }
}