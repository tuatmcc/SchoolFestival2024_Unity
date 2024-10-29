using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;

namespace RicoShot.VFX
{
    public class VFXTrail1 : MonoBehaviour
    {
        [SerializeField] private VisualEffect vfx;
        private const string RatePropertyName = "RatePerUnit";
        [SerializeField] private Rigidbody targetRb;
        [SerializeField] private float minimumRate = 1.0f;
        [SerializeField] private float strength = 1.0f;
        
        private void FixedUpdate()
        {
            var rate = targetRb.velocity.magnitude;
            rate = Mathf.Max(minimumRate, rate) - minimumRate;
            rate *= strength;
            vfx.SetFloat(RatePropertyName, rate);
        }
    }
}
