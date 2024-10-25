using UnityEngine;

namespace Chibi
{
    [RequireComponent(typeof(CharacterChibiSwitcher))]
    public class CharacterCameraTargets : MonoBehaviour
    {
        [SerializeField] private Transform headTarget;
        [SerializeField] private Transform leftHandTarget;
        [SerializeField] private Transform rightHandTarget;

        private CharacterChibiSwitcher chibiSwitcher;

        private void Awake()
        {
            chibiSwitcher = GetComponent<CharacterChibiSwitcher>();
        }

        private void Update()
        {
            headTarget.position = chibiSwitcher.current.headTransform.position;
            headTarget.rotation = chibiSwitcher.current.headTransform.rotation;
            leftHandTarget.position = chibiSwitcher.current.leftHandTransform.position;
            leftHandTarget.rotation = chibiSwitcher.current.leftHandTransform.rotation;
            rightHandTarget.position = chibiSwitcher.current.rightHandTransform.position;
            rightHandTarget.rotation = chibiSwitcher.current.rightHandTransform.rotation;
        }
    }
}