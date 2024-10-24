using UnityEngine;

namespace Chibi.ChibiComponents
{
    [RequireComponent(typeof(ChibiSettings))]
    [RequireComponent(typeof(Animator))]
    public class Chibi : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private Transform head;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;
        [SerializeField] private ChibiSettings _chibiSettings;

        public Animator animator => _animator;
        public ChibiSettings chibiSettings => _chibiSettings;

        public Avatar avatar => animator.avatar;
        public Transform headTransform => head;
        public Transform leftHandTransform => leftHand;
        public Transform rightHandTransform => rightHand;
    }
}