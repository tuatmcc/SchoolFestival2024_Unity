using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TImeTextController: MonoBehaviour
    {
        private TextMeshProUGUI text;
        
        [Inject] private ITimeController timeController;

        private void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            timeController.OnTimeChangedSeconds += OnTimeChangedSeconds;
        }
        
        private void OnTimeChangedSeconds(int time)
        {
            text.text = (time / 60).ToString() + ":" + (time % 60).ToString().PadLeft(2,'0');
        }
        
        private void OnDestroy()
        {
            timeController.OnTimeChangedSeconds -= OnTimeChangedSeconds;
        }
    }
}