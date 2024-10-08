using RicoShot.Core;
using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Matching.Tests
{
    public class AllReadyPresenter : MonoBehaviour
    {
        private TextMeshProUGUI textMeshProUGUI;

        [Inject] private readonly INetworkController networkController;

        void Start()
        {
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            networkController.OnAllClientsReadyChanged += OnAllReadyChanged;
        }

        void OnAllReadyChanged(bool allReady)
        {
            if (allReady)
            {
                textMeshProUGUI.text = "AllReady: true";
            }
            else
            {
                textMeshProUGUI.text = "AllReady: false";
            }
        }
    }
}
