using Cysharp.Threading.Tasks;
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

        private void Update()
        {
            transform.LookAt(_playSceneManager.MainCameraTransform);
        }
    }
}