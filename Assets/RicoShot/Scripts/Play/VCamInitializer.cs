using Cinemachine;
using RicoShot.Play.Interface;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
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

        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera playVCam;
        [SerializeField] private CinemachineVirtualCamera spawnVCam;

        private void Start()
        {
            playSceneManager.OnLocalPlayerSpawned += x => ChangeCamera(x).Forget();

            if (playSceneTester.IsTest)
            {
                playSceneManager.MainCameraTransform = mainCamera.transform;
                playSceneManager.VCamTransform = playVCam.transform;
                playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
                return;
            }

            if (NetworkManager.Singleton.IsClient)
            {
                playSceneManager.MainCameraTransform = mainCamera.transform;
                playSceneManager.VCamTransform = playVCam.transform;
                playSceneManager.OnLocalPlayerSpawned += SetLocalPlayerTransform;
                Debug.Log("Set VCamTransform finished");
            }

            if (NetworkManager.Singleton.IsServer) gameObject.SetActive(false);
        }

        private void SetLocalPlayerTransform(GameObject localPlayer)
        {
            playVCam.Follow = localPlayer.transform;
            playVCam.LookAt = localPlayer.transform;
            spawnVCam.Follow = localPlayer.transform;
            spawnVCam.LookAt = localPlayer.transform;
            Debug.Log("VCam setting finished");
        }

        private async UniTaskVoid ChangeCamera(GameObject localPlayer)
        {
            await UniTask.Delay(3000);
            spawnVCam.gameObject.SetActive(false);
        }
    }
}