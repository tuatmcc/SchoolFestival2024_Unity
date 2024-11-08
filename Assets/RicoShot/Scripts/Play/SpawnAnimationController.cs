using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RicoShot.Play
{
    [RequireComponent(typeof(Animator))]
    public class SpawnAnimationController : MonoBehaviour
    {
        private const int SpawnAnimationClipTime = 3000;

        [SerializeField] private Animator balloon;
        [SerializeField] private AudioClip landingAudioClip;

        private readonly int _animIDSpawn = Animator.StringToHash("Spawn");
        private readonly int _animIDSpawnClipIndex = Animator.StringToHash("SpawnClipIndex");
        private Animator _animator;
        private bool _spawn;
        private int _spawnClipIndex;

        public bool isSpawning { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            balloon.gameObject.SetActive(false);
        }

        private void Start()
        {
            // Debug
            Spawn();
        }

        private void LateUpdate()
        {
            _animator.SetBool(_animIDSpawn, _spawn);
            _animator.SetInteger(_animIDSpawnClipIndex, _spawnClipIndex);
            balloon.SetInteger(_animIDSpawnClipIndex, _spawnClipIndex);
        }

        public void Spawn()
        {
            if (isSpawning) return;
            SpawnAsync().Forget();
        }

        private async UniTaskVoid SpawnAsync()
        {
            _spawn = true;
            isSpawning = true;
            // 0 or 1, but 0 should be more likely
            _spawnClipIndex = Random.value > 0.2f ? 0 : 1;
            balloon.gameObject.SetActive(true);
            // 強引だが、1フレームの間にアニメーションをトリガーするのに失敗することがあったっぽいので、、、
            UniTask.Create(async () =>
            {
                await UniTask.DelayFrame(10, cancellationToken: destroyCancellationToken);
                _spawn = false;
            }).Forget();
            await UniTask.Delay(SpawnAnimationClipTime, cancellationToken: destroyCancellationToken);
            isSpawning = false;
            balloon.gameObject.SetActive(false);
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            // TODO: Play the sound effect
            Debug.Log("OnLand");
        }
    }
}