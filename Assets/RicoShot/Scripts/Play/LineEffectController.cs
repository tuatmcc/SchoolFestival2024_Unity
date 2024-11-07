using System;
using R3;
using RicoShot.Play.Interface;
using UnityEngine;
using UnityEngine.VFX;
using Zenject;

namespace RicoShot.Play
{
    public class LineEffectController : MonoBehaviour
    {
        private const string Mesh = "LineMesh";
        private VisualEffect _vfx;
        private int _meshId;
        
        [Inject] private IPlaySceneManager _playSceneManager;

        private void Start()
        {
            _vfx = GetComponent<VisualEffect>();
            _meshId = Shader.PropertyToID(Mesh);
            _playSceneManager.OnLocalPlayerSpawned += LineEffect;
        }

        private void OnDisable()
        {
            _playSceneManager.OnLocalPlayerSpawned -= LineEffect;
        }

        private void LineEffect(GameObject localPlayer)
        {
            var bulletRayManager = localPlayer.GetComponent<BulletRayManager>();
            bulletRayManager.OnDrawRayAsObservable
                .Subscribe(mesh =>
                {
                    _vfx.SetMesh(_meshId, mesh);
                    _vfx.enabled = true;
                }).AddTo(this);
            bulletRayManager.OnCancelDrawRayAsObservable
                .Subscribe(_ =>
                {
                    _vfx.enabled = false;
                }).AddTo(this);
        }
    }
}
