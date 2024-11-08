using System;
using DG.Tweening;
using R3;
using RicoShot.Play.Interface;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TimeLimitPresenter : MonoBehaviour
    {
        [Inject] private readonly ITimeManager _timeManager;
        [SerializeField] private TMP_Text timeLimitText;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();

            // hide to the left
            _rectTransform.anchoredPosition = new Vector2(-_rectTransform.rect.width, 352);

            // show this presenter during countdown
            Observable.FromEvent<int>(h => _timeManager.OnCountChanged += h,
                    h => _timeManager.OnCountChanged -= h)
                .Where(count => count == 4).Subscribe(_ => ShowTimeLimitPresenter()).AddTo(this);

            // update time limit
            Observable.FromEvent<long>(h => _timeManager.OnPlayTimeChanged += h,
                    h => _timeManager.OnPlayTimeChanged -= h)
                .Subscribe(OnPlayTimeChanged).AddTo(this);
        }

        private void OnPlayTimeChanged(long playTime)
        {
            var time = TimeSpan.FromTicks(playTime);
            timeLimitText.text = $"{time.Minutes:0}:{time.Seconds:00}";
        }

        private void ShowTimeLimitPresenter()
        {
            _rectTransform.DOAnchorPosX(0, 0.3f).SetEase(Ease.OutBack);
        }
    }
}