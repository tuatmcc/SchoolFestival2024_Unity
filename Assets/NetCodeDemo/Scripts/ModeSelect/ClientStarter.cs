using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class ClientStarter : MonoBehaviour
{
   public void OnClick()
   {
      NetworkManagerController.StartClient();
      SceneManager.LoadScene("Server"); 
   }
}
