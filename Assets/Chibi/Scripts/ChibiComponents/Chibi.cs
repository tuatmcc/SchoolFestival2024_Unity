using UnityEngine;

namespace Chibi.ChibiComponents
{
    [RequireComponent(typeof(ChibiSettings))]
    [RequireComponent(typeof(Animator))]
    public class Chibi : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform headBone;
        [SerializeField] private Transform neckBone;
        [SerializeField] private Transform leftHandBone;
        [SerializeField] private Transform rightHandBone;
        [SerializeField] private ChibiSettings _chibiSettings;

        public Animator animator => _animator;
        public ChibiSettings chibiSettings => _chibiSettings;

        public Avatar avatar => animator.avatar;
        public Transform headTransform => headBone;

        public Transform neckTransform => neckBone;
        public Transform leftHandTransform => leftHandBone;
        public Transform rightHandTransform => rightHandBone;
    }
}