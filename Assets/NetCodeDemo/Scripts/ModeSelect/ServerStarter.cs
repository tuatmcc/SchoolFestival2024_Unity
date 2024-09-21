using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerStarter : MonoBehaviour
{
   public void OnClick()
   {
      NetworkManagerController.StartServer();
      SceneManager.LoadScene("Server");
   }
}
