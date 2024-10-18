using RicoShot.InputActions;
using System;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface IPlaySceneManager
    {
        public event Action<GameObject> OnLocalPlayerSpawned; 

        public PlayInputs PlayInputs { get; }
        public GameObject LocalPlayer { get; set; }
    }
}