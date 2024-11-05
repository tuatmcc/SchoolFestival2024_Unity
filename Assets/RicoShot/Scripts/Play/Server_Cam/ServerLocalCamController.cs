using System.Collections;
using System.Collections.Generic;
using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine;
using Zenject;

public class ServerLocalCamController : MonoBehaviour
{
    [Inject] private readonly IGameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameStateManager.NetworkMode == NetworkMode.Server)
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
