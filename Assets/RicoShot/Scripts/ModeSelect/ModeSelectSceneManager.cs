using Cysharp.Threading.Tasks;
using RicoShot.Core;
using RicoShot.Core.Interface;
using RicoShot.InputActions;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace RicoShot.ModeSelect
{
    public class ModeSelectSceneManager : IInitializable, IDisposable
    {
        public ModeSelectInputs ModeSeletotInputs { get; private set; }

        private bool selected = false;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ISupabaseController supabaseController;

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
            if (selected) return;
            selected = true;
            gameStateManager.NetworkMode = networkMode;
            Debug.Log($"Set NetworkMode {networkMode}");
            WaitSupabaseConnect().Forget();
        }

        // Supabaseへの接続を待って次のシーンへ
        private async UniTask WaitSupabaseConnect()
        {
            await supabaseController.Connect();
            gameStateManager.NextScene();
        }

        public void Dispose()
        {
            ModeSeletotInputs.Disable();
        }
    }
}
