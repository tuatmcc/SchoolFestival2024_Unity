using System;
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

        private void Start()
        {
            var countdownObservable = Observable.FromEvent<int>(h => _timeManager.OnCountChanged += h,
                h => _timeManager.OnCountChanged -= h);
            countdownObservable.Where(count => count == 1).Subscribe(_ => ShowReady()).AddTo(this);
            countdownObservable.Where(count => count == 0).Subscribe(_ => ShowGo()).AddTo(this);

            readyImage.rectTransform.anchoredPosition = new Vector2(0, -Screen.height);
            goImage.rectTransform.anchoredPosition = new Vector2(0, -Screen.height);
        }

        private void ShowReady()
        {
            readyImage.rectTransform.DOAnchorPos(new Vector2(0, 0), 0.3f).SetEase(Ease.OutBack);
        }

        private void ShowGo()
        {
            readyImage.rectTransform.DOAnchorPos(new Vector2(0, Screen.height), 0.3f).SetEase(Ease.InBack);
            goImage.rectTransform.DOAnchorPos(new Vector2(0, 0), 0.3f).SetEase(Ease.OutBack);

            Observable.Timer(TimeSpan.FromSeconds(1)).Subscribe(_ =>
                    goImage.rectTransform.DOAnchorPos(new Vector2(0, Screen.height), 0.3f).SetEase(Ease.InBack))
                .AddTo(this);
        }
    }
}