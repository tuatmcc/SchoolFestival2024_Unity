using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using UnityEngine;
using Zenject;

namespace RicoShot.ModeSelect
{
    public class ModeSelectSceneManager : IInitializable, IDisposable
    {
        public ModeSelectInputs ModeSeletotInputs { get; private set; }

        [Inject] private IGameStateManager gameStateManager;

        ModeSelectSceneManager()
        {
            ModeSeletotInputs = new();
            ModeSeletotInputs.Enable();
        }

        public void Initialize()
        {

        }

        public void SetNetworkMode(NetworkMode networkMode)
        {
            gameStateManager.NetworkMode = networkMode;
            Debug.Log($"Set NetworkMode {networkMode}");
        }

        public void Dispose()
        {
            ModeSeletotInputs.Dispose();
        }
    }
}
