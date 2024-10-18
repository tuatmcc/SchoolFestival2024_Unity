using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RicoShot.Play.Interface;
using System;
using RicoShot.InputActions;

namespace RicoShot.Play
{
    public class PlaySceneManager : IPlaySceneManager, IDisposable
    {
        public event Action<GameObject> OnLocalPlayerSpawned;

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

        private GameObject localPlayer;

        PlaySceneManager()
        {
            PlayInputs = new();
            PlayInputs.Enable();
        }

        public void Dispose()
        {
            PlayInputs.Dispose();
        }
    }
}
