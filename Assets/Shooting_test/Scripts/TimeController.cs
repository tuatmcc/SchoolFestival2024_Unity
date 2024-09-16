using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            StartCoroutine(BattleTimer());
        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator BattleTimer()
        {
            while (time_second > 0)
            {
                Timer_TMP.text = Seconds2SecondsAndMinutes(time_second);
                yield return new WaitForSeconds(1);
                time_second--;
            }
        }

        string Seconds2SecondsAndMinutes(int seconds)
        {
            return (seconds / 60).ToString() + ":" + (seconds % 60).ToString().PadLeft(2,'0');
        }
    }
}