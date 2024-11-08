using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.Play.UI
{
    [RequireComponent(typeof(Canvas))]
    public class SceneUI : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private LocalPlayerMoveController playerMoveController;

        [Inject] private readonly IPlaySceneManager _playSceneManager;
        [Inject] private readonly ILocalPlayerManager _localPlayerManager;

        private void Start()
        {
            Observable.FromEvent<int>(h => playerMoveController.OnHpChanged += h,
                    h => playerMoveController.OnHpChanged -= h)
                .Subscribe(x => slider.value = (float)x / LocalPlayerMoveController.MaxHp).AddTo(this);
        }

        private void LateUpdate()
        {
            transform.forward = _playSceneManager.MainCameraTransform.forward;
        }
    }
}