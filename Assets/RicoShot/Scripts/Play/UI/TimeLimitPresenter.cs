using System;
using R3;
using RicoShot.Play.Interface;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TimeLimitPresenter : MonoBehaviour
    {
        [Inject] private readonly ITimeManager _timeManager;
        private TMP_Text _timeLimitText;

        private void Start()
        {
            _timeLimitText = GetComponent<TMP_Text>();
            Observable.FromEvent<long>(h => _timeManager.OnPlayTimeChanged += h,
                    h => _timeManager.OnPlayTimeChanged -= h)
                .Subscribe(OnPlayTimeChanged).AddTo(this);
        }

        private void OnPlayTimeChanged(long playTime)
        {
            var time = TimeSpan.FromTicks(playTime);
            _timeLimitText.text = $"{time.Minutes:0}:{time.Seconds:00}";
        }
    }
}