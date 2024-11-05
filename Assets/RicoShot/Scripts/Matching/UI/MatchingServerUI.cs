using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.Matching.UI
{
    public class MatchingServerUI : MonoBehaviour
    {
        [Inject] private IGameStateManager _gameStateManager;
        [SerializeField] private RectTransform serverUI;

        private void Start()
        {
            serverUI.gameObject.SetActive(_gameStateManager.NetworkMode == NetworkMode.Server);
        }
    }
}