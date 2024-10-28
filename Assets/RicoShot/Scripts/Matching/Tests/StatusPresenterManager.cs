using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoShot.Matching.Tests
{
    public class StatusPresenterManager : MonoBehaviour
    {
        [SerializeField] List<TextMeshProUGUI> textMeshProList;
        [Inject] INetworkController networkController;

        void Start()
        {
            networkController.ClientDataList.OnDataChanged += UpdateStatusText;
        }

        private void UpdateStatusText()
        {
            for(int i = 0; i < networkController.ClientDataList.Count; i++)
            {
                var data = networkController.ClientDataList[i];
                textMeshProList[i].text = $"{data.Name} | {data.Team} | {data.IsReady}";
            }
            for (int i = networkController.ClientDataList.Count; i < textMeshProList.Count; i++)
            {
                textMeshProList[i].text = $"Name | Team | IsReady";
            }
        }
    }
}
