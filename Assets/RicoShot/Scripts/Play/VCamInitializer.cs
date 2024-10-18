using Cinemachine;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class VCamInitializer : MonoBehaviour
{
    [Inject] private readonly IPlaySceneManager playSceneManager;

    private void Start()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            if (playSceneManager.LocalPlayer != null)
            {
                SetLocalPlayerTransform(playSceneManager.LocalPlayer);
            }
            playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
        }
    }

    private void SetLocalPlayerTransform(GameObject localPlayer)
    {
        var cvc = GetComponent<CinemachineVirtualCamera>();
        cvc.Follow = localPlayer.transform;
        cvc.LookAt = localPlayer.transform;
        Debug.Log(localPlayer);       
    }
}
