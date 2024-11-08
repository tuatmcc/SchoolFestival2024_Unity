using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using R3;
using RicoShot.Play.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.Play.UI
{
    public class DisplayMessagePresenter : MonoBehaviour
    {
        [Inject] private readonly ITimeManager _timeManager;
        [SerializeField] private Image readyImage;
        [SerializeField] private Image goImage;

        [SerializeField] private float readyStartY = -1080f;
        [SerializeField] private float goStartY = -1080f;
        [SerializeField] private float readyEndY = 1080f;
        [SerializeField] private float goEndY = 1080f;

        [SerializeField] private Image[] countDownImages;
        private int _countDownImageIndex = 0;

        private void Start()
        {
            var countdownObservable = Observable.FromEvent<int>(h => _timeManager.OnCountChanged += h,
                h => _timeManager.OnCountChanged -= h);

            // count down
            countdownObservable.Subscribe(CountDown).AddTo(this);

            // readyImage.rectTransform.anchoredPosition = new Vector2(0, -readyStartY);
            // goImage.rectTransform.anchoredPosition = new Vector2(0, -goStartY);
            foreach (var image in countDownImages) image.rectTransform.anchoredPosition = new Vector2(0, -1080f);
        }

        private void ShowReady()
        {
            readyImage.rectTransform.DOAnchorPos(new Vector2(0, 0), 0.3f).SetEase(Ease.OutBack);
        }

        private void ShowGo()
        {
            readyImage.rectTransform.DOAnchorPos(new Vector2(0, readyEndY), 0.3f).SetEase(Ease.InBack);
            goImage.rectTransform.DOAnchorPos(new Vector2(0, 0), 0.3f).SetEase(Ease.OutBack);

            UniTask.Create(async () =>
            {
                await UniTask.Delay(1000);
                goImage.rectTransform.DOAnchorPos(new Vector2(0, goEndY), 0.3f).SetEase(Ease.InBack);
            });
        }

        private void CountDown(int count)
        {
            if (count < countDownImages.Length)
                UniTask.Create(async () =>
                {
                    countDownImages[_countDownImageIndex].rectTransform.DOAnchorPos(new Vector2(0, 0), 0.2f)
                        .SetEase(Ease.OutBack);
                    await UniTask.Delay(600);
                    countDownImages[_countDownImageIndex++].rectTransform.DOAnchorPos(new Vector2(0, 1080), 0.2f)
                        .SetEase(Ease.InBack);
                });
        }
    }
}