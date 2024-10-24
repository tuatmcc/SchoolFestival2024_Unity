using UnityEngine;

namespace Chibi
{
    [RequireComponent(typeof(ChibiSettings))]
    [RequireComponent(typeof(Animator))]
    public class Chibi : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ChibiSettings _chibiSettings;

        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        public Animator animator => _animator;
        public ChibiSettings chibiSettings => _chibiSettings;

        public Avatar avatar => animator.avatar;
        public Transform headTransform => head;
        public Transform leftHandTransform => leftHand;
        public Transform rightHandTransform => rightHand;
    }
}