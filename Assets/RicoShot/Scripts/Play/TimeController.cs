using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using Zenject;

namespace RicoShot.Play
{
    public class TimeController : ITimeController, IInitializable, IDisposable
    {
        private CancellationTokenSource cts;
        private int time_second = ITimeController.TIME_LIMIT;
        public event Action<int> OnTimeChangedSeconds;
        public event Action OnTimeOver;

        public void Initialize()
        {
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
            }).Forget();
        }

        public void Dispose()
        {
            cts.Cancel();
        }
    }
}