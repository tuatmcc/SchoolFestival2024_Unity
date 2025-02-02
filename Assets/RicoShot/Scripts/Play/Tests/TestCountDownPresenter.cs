using RicoShot.Play.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Play.Tests
{
    public class TestCountDownPresenter : MonoBehaviour
    {
        private TextMeshProUGUI countText;

        [Inject] private readonly ITimeManager timeManager;
        [Inject] private readonly IPlaySceneTester playSceneTester;

        private void Start()
        {
            countText = GetComponent<TextMeshProUGUI>();
            if (playSceneTester.IsTest)
            {
                countText.enabled = false;
                return;
            }
            countText.text = "0";
            timeManager.OnCountChanged += OnCountChanged;
            timeManager.OnPlayTimeChanged += OnPlayTimeChanged;
        }

        private void OnCountChanged(int count)
        {
            countText.text = $"{count}";
        }

        private void OnPlayTimeChanged(long playTime)
        {
            var time = TimeSpan.FromTicks(playTime);
            countText.text = $"{time.Minutes:00}:{time.Seconds:00}";
        }
    }
}
