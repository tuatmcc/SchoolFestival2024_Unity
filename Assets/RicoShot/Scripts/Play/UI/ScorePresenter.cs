using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.Play.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.Play.UI
{
    public class ScorePresenter : MonoBehaviour
    {
        [SerializeField] private Image scoreImage;
        [SerializeField] private TMP_Text scoreText;

        [Inject] private readonly INetworkScoreManager _scoreManager;
        [Inject] private readonly ILocalPlayerManager _localPlayerManager;
        [Inject] private readonly IPlaySceneManager _playSceneManager;
        [Inject] private readonly IGameStateManager _gameStateManager;

        private void Start()
        {
            if (_gameStateManager.NetworkMode == NetworkMode.Client)
            {
                scoreImage.enabled = true;
                scoreText.enabled = true;
                scoreText.text = "0";
                Observable.FromEvent<PlayState>(h => _playSceneManager.OnPlayStateChanged += h,
                        h => _playSceneManager.OnPlayStateChanged -= h)
                    .Where(state => state == PlayState.Playing)
                    .Subscribe(_ => SetScoreUpdater())
                    .AddTo(this);
            }
            else
            {
                scoreImage.enabled = false;
                scoreText.enabled = false;
            }
        }

        private void SetScoreUpdater()
        {
            var index = _scoreManager.ScoreList.FindIndex(x => x.UUID.ToString() == _localPlayerManager.LocalPlayerUUID);
            Debug.Log(index);
            Observable.FromEvent(h => _scoreManager.ScoreList.OnDataChanged += h,
                    h => _scoreManager.ScoreList.OnDataChanged -= h)
                .Subscribe(_ => scoreText.text = _scoreManager.ScoreList[index].Score.ToString())
                .AddTo(this);
        }
    }
}