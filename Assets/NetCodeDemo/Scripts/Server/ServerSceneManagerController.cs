using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ServerSceneManagerController : NetworkBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject text;
    
    void Start()
    {
        if (NetworkManager.Singleton.IsClient)
        {
            button.SetActive(false);
            text.SetActive(true);
        }
        else
        {
            button.SetActive(true);
            text.SetActive(false);
        }    
    }
}
