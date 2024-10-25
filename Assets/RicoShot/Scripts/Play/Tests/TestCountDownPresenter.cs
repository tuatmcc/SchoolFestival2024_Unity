using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class TestCountDownPresenter : MonoBehaviour
{
    private TextMeshProUGUI countText;

    [Inject] private readonly ITimeManager timeManager;

    private void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();
        countText.text = "0";
        timeManager.OnCountChanged += OnCountChanged;
    }

    private void OnCountChanged(int count)
    {
        countText.text = $"{count}";
    }
}
