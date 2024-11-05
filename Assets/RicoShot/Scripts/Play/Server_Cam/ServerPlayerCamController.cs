using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine;
using Zenject;
using Cinemachine;
using System;

namespace RicoShot {
    public class ServerPlayerCamController : MonoBehaviour
    {
        int ChangeInterval = 10;
        bool isEdited = false;
        [Inject] private readonly IGameStateManager gameStateManager;
        // Start is called before the first frame update
        void Start()
        {
            if(gameStateManager.NetworkMode == NetworkMode.Client)
            {
                this.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(Time.time);
            Debug.Log(GetComponent<CinemachineVirtualCamera>().Priority);
            if (DateTime.Now.Second % ChangeInterval == 0 && !isEdited)
            {
                GetComponent<CinemachineVirtualCamera>().Priority = UnityEngine.Random.Range(0, 11);
                isEdited = true;
            }
            else if(DateTime.Now.Second % ChangeInterval != 0)
            {
                isEdited = false;
            }
        }
    }
}