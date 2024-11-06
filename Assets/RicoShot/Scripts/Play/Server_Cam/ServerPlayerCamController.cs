using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine;
using Zenject;
using Cinemachine;
using System;

namespace RicoShot {
    public class ServerPlayerCamController : MonoBehaviour
    {
        private int ChangeInterval = 10;
        private bool isEdited = false;
        private CinemachineVirtualCamera virtualCamera;

        [Inject] private readonly IGameStateManager gameStateManager;
        
        void Start()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
            if(gameStateManager.NetworkMode == NetworkMode.Client)
            {
                this.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Time.time);
            //Debug.Log(virtualCamera.Priority);
            if (DateTime.Now.Second % ChangeInterval == 0 && !isEdited)
            {
                virtualCamera.Priority = UnityEngine.Random.Range(0, 11);
                isEdited = true;
            }
            else if(DateTime.Now.Second % ChangeInterval != 0)
            {
                isEdited = false;
            }
        }
    }
}