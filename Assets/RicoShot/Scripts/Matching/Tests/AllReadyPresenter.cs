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
            networkController.OnAllClientsReady += OnAllReady;
            networkController.OnAllClientsReadyCancelled += OnAllReadyCancelled;
        }

        void OnAllReady()
        {
            textMeshProUGUI.text = "AllReady: true";
        }

        void OnAllReadyCancelled()
        {
            textMeshProUGUI.text = "AllReady: false";
        }
    }
}
