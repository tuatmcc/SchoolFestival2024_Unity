using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System;

namespace Shooting_test
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField]
        public TextMeshProUGUI Timer_TMP;

        int time_second = 180;
        // Start is called before the first frame update
        void Start()
        {
            BattleTimer();
        }

        // Update is called once per frame
        void Update()
        {

        }
        /*
        IEnumerator BattleTimer()
        {
            while (time_second > 0)
            {
                Timer_TMP.text = Seconds2SecondsAndMinutes(time_second);
                yield return new WaitForSeconds(1);
                time_second--;
            }
        }
        */

        async void BattleTimer()
        {
            while (time_second > 0)
            {
                Timer_TMP.text = Seconds2SecondsAndMinutes(time_second);
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                time_second--;
            }
        }

        string Seconds2SecondsAndMinutes(int seconds)
        {
            return (seconds / 60).ToString() + ":" + (seconds % 60).ToString().PadLeft(2,'0');
        }
    }
}