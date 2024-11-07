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
        [SerializeField] private CinemachineVirtualCamera fieldVCam;
        [SerializeField] private CinemachineTargetGroup targetGroup;

        private void Start()
        {
            playSceneManager.OnLocalPlayerSpawned += x => StartSpawnCameraAsync(x).Forget();
            playSceneManager.OnLocalPlayerSpawned += _ => StartFieldCameraAsync().Forget();

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
            playVCam.transform.position = localPlayer.transform.position;
            playVCam.m_Follow = localPlayer.transform;
            playVCam.m_LookAt = localPlayer.transform;
            spawnVCam.Follow = localPlayer.transform;
            spawnVCam.LookAt = localPlayer.transform;
            Debug.Log("VCam setting finished");
        }

        private async UniTaskVoid StartSpawnCameraAsync(GameObject localPlayer)
        {
            await UniTask.Delay(2500, cancellationToken: this.GetCancellationTokenOnDestroy());
            spawnVCam.gameObject.SetActive(false);
        }

        private async UniTaskVoid StartFieldCameraAsync()
        {
            targetGroup.m_Targets[0].target = playSceneManager.LocalPlayer.transform;
            await UniTask.Delay(5000, cancellationToken: this.GetCancellationTokenOnDestroy());
            fieldVCam.gameObject.SetActive(false);
        }
    }
}