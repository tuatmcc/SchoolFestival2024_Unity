using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class NetworkManagerController : MonoBehaviour
{
    static public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    static public void StartClient()
    {
        var transport = NetworkManager.Singleton.NetworkConfig.NetworkTransport;
        if (transport is Unity.Netcode.Transports.UTP.UnityTransport utpTransport)
        {
            utpTransport.SetConnectionData(Convert.ToString("127.0.0.1"), 12345);
        }
        NetworkManager.Singleton.StartClient();
    }

    static public void LoadScene(String sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
