using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using RicoShot.Play.Interface;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Play
{
    public class TimeController : ITimeController, IInitializable, IDisposable
    {
        private readonly CompositeDisposable disposables = new();
        private IPlaySceneManager _playSceneManager;
        private CancellationTokenSource cts;
        private int time_second = ITimeController.TIME_LIMIT;

        public void Dispose()
        {
            cts.Cancel();
            disposables.Dispose();
        }

        public void Initialize()
        {
            // なんか動かないなぁと思ったらこのクラスじゃなくてTestTimeControllerを使ってたみたい
            Observable.FromEvent<InputAction.CallbackContext>(
                    action => _playSceneManager.PlayInputs.Main.Skip.performed += action,
                    action => _playSceneManager.PlayInputs.Main.Skip.performed -= action)
                .Subscribe(SkipTime)
                .AddTo(disposables);

            cts = new CancellationTokenSource();
            UniTask.Create(async () =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        OnTimeChangedSeconds?.Invoke(time_second);
                        await UniTask.Delay(1000, cancellationToken: cts.Token);
                        time_second--;
                        if (time_second < 0)
                        {
                            OnTimeOver?.Invoke();
                            break;
                        }
                    }

                    return UniTask.CompletedTask;
                })
                .Forget();
        }

        public event Action<int> OnTimeChangedSeconds;
        public event Action OnTimeOver;

        private void SkipTime(InputAction.CallbackContext context)
        {
            cts.Cancel();
            OnTimeOver?.Invoke();
        }
    }
}