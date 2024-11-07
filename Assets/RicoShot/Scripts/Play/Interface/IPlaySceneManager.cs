using RicoShot.InputActions;
using System;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface IPlaySceneManager
    {
        public event Action<PlayState> OnPlayStateChanged;
        public event Action<GameObject> OnLocalPlayerSpawned;

        public PlayState PlayState { get; set; }
        public PlayInputs PlayInputs { get; }
        public GameObject LocalPlayer { get; set; }
        public Transform VCamTransform { get; set; }
        public Transform MainCameraTransform { get; set; }
    }
}