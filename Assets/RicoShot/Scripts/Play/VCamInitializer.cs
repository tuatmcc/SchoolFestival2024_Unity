using Cinemachine;
using RicoShot.Play.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace RicoShot.Play
{
    /// <summary>
    /// PlaySceneManagerに自身のTransformを登録
    /// LocalPlayerがSpawnしたらFollowとLookAtに登録
    /// </summary>
    public class VCamInitializer : MonoBehaviour
    {
        [Inject] private readonly IPlaySceneManager playSceneManager;

        private void Awake()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                playSceneManager.VCamTransform = transform;
                playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
                Debug.Log("Set VCamTransform finished");
            }
        }

        private void SetLocalPlayerTransform(GameObject localPlayer)
        {
            var cvc = GetComponent<CinemachineVirtualCamera>();
            cvc.Follow = localPlayer.transform;
            cvc.LookAt = localPlayer.transform;
            Debug.Log("VCam setting finished");
        }
    }
}
