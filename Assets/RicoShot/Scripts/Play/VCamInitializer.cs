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
        [Inject] private readonly IPlaySceneTester playSceneTester;

        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        private void Start()
        {
            if (playSceneTester.IsTest)
            {
                playSceneManager.VCamTransform = virtualCamera.transform;
                playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
                return;
            }

            if (NetworkManager.Singleton.IsClient)
            {
                playSceneManager.VCamTransform = virtualCamera.transform;
                playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
                Debug.Log("Set VCamTransform finished");
            }

            if (NetworkManager.Singleton.IsServer) gameObject.SetActive(false);
        }

        private void SetLocalPlayerTransform(GameObject localPlayer)
        {
            virtualCamera.Follow = localPlayer.transform;
            virtualCamera.LookAt = localPlayer.transform;
            Debug.Log("VCam setting finished");
        }
    }
}