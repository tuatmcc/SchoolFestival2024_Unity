using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Play.Interface;
using System;
using RicoShot.InputActions;
using Zenject;

namespace RicoShot.Play
{
    public class PlaySceneManager : IPlaySceneManager, IInitializable, IDisposable
    {
        public event Action<PlayState> OnPlayStateChanged;
        public event Action<GameObject> OnLocalPlayerSpawned;

        public PlayState PlayState
        {
            get => playState;
            set
            {
                playState = value;
                OnPlayStateChanged?.Invoke(playState);
            }
        }

        public PlayInputs PlayInputs { get; private set; }
        public GameObject LocalPlayer
        {
            get => localPlayer;
            set
            {
                localPlayer = value;
                OnLocalPlayerSpawned?.Invoke(localPlayer);
            }
        }
        public Transform VCamTransform { get; set; }

        private PlayState playState;
        private GameObject localPlayer;

        PlaySceneManager()
        {
            PlayInputs = new();
            PlayInputs.Enable();
        }

        public void Initialize()
        {
            PlayState = PlayState.Waiting;
        }

        public void Dispose()
        {
            PlayInputs.Dispose();
        }
    }
}
