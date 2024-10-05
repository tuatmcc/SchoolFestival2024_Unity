using RicoShot.Core.Interface;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class StatusPresenterManager : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> textMeshProList;
    [Inject] INetworkController networkController;

    void Start()
    {
        networkController.OnClientDatasChanged += UpdateStatusText;
    }

    private void UpdateStatusText()
    {
        for(int i = 0; i < networkController.ClientDatas.Count; i++)
        {
            var data = networkController.ClientDatas[i];
            textMeshProList[i].text = $"{data.Name} | {data.Team} | {data.IsReady}";
        }
    }
}
