using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine;
using Zenject;
using Cinemachine;
using System;
using System.Collections.Generic;
using R3;
using RicoShot.Play.Interface;

namespace RicoShot.Play
{
    public class ServerCamController : MonoBehaviour
    {
        private int ChangeInterval = 10;
        private bool isEdited = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly IPlaySceneManager _playSceneManager;
        [Inject] private readonly IPlaySceneTester _playSceneTester;
        [Inject] private readonly ICharacterGenerator _characterGenerator;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera fieldCamera;
        [SerializeField] private CinemachineVirtualCamera[] playerCameras;

        private Transform[] _players;

        private readonly List<CinemachineVirtualCamera> _serverCameras = new();

        private void Start()
        {
            if (gameStateManager.NetworkMode == NetworkMode.Client) gameObject.SetActive(false);
            if (_playSceneTester.IsTest) gameObject.SetActive(false);

            if (gameStateManager.NetworkMode == NetworkMode.Server)
            {
                _playSceneManager.MainCameraTransform = mainCamera.transform;

                Observable.FromEvent<PlayState>
                    (h => _playSceneManager.OnPlayStateChanged += h,
                        h => _playSceneManager.OnPlayStateChanged -= h).Where(x => x == PlayState.Playing)
                    .Subscribe(_ => SetupCameraList()).AddTo(this);

                Observable.Interval(TimeSpan.FromSeconds(10f)).Subscribe(_ =>
                {
                    _serverCameras.ForEach(x => x.Priority = UnityEngine.Random.Range(0, 11));
                }).AddTo(this);
            }
        }

        private void SetupCameraList()
        {
            // Why do I have to do this here? Can't I do this at the Inspector?
            fieldCamera.m_LookAt = transform;
            _serverCameras.Add(fieldCamera);

            if (_characterGenerator.PlayerTransforms.Count != playerCameras.Length)
            {
                Debug.LogError("PlayerTransforms.Length != playerCameras.Length: " +
                               _characterGenerator.PlayerTransforms.Count + " != " + playerCameras.Length);
                return;
            }

            for (var i = 0; i < playerCameras.Length; i++)
            {
                playerCameras[i].m_LookAt = _characterGenerator.PlayerTransforms[i];
                _serverCameras.Add(playerCameras[i]);
            }

            Debug.Log("SetPlayerCameras finished CameraCount: " + _serverCameras.Count);
        }
    }
}