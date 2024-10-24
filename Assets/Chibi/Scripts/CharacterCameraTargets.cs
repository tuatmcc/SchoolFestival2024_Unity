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
            headTarget.transform.position = chibiSwitcher.current.headTransform.transform.position;
            leftHandTarget = chibiSwitcher.current.leftHandTransform;
            rightHandTarget = chibiSwitcher.current.rightHandTransform;
        }
    }
}