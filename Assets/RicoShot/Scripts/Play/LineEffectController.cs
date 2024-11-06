using R3;
using UnityEngine;
using UnityEngine.VFX;

namespace RicoShot.Play
{
    public class LineEffectController : MonoBehaviour
    {
        private const string Mesh = "LineMesh";
        private void Start()
        {
            var bulletRayManager = GetComponent<BulletRayManager>();
            var vfx = GetComponent<VisualEffect>();
            var meshId = Shader.PropertyToID(Mesh);
            bulletRayManager.OnDrawRayAsObservable
                .Subscribe(mesh =>
                {
                    vfx.SetMesh(meshId, mesh);
                    vfx.enabled = true;
                });
            bulletRayManager.OnCancelDrawRayAsObservable
                .Subscribe(_ =>
                {
                    vfx.enabled = false;
                });
        }
    }
}
