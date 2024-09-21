using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    public void OnClick()
    {
        NetworkManagerController.LoadScene("ConnectionDemo");
    }
}
