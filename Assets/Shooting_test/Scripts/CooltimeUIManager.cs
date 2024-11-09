using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.InputSystem;

namespace Shooting_test
{
    public class CooltimeUIManager : MonoBehaviour
    {
        private bool OnCooltime = false;

        private int COOLTIME = 2;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        // async public void Begin_Cooltime(InputAction.CallbackContext context)
        // {
        //     if (context.performed && !OnCooltime)
        //     {
        //         OnCooltime = true;
        //         this.GetComponent<Image>().fillAmount = 1f;
        //         await this.GetComponent<Image>().DOFillAmount(0f, COOLTIME).SetEase(Ease.Linear);
        //         OnCooltime = false;
        //     }
        // }
    }
}