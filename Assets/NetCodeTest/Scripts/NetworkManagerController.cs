using Unity.Netcode;
using UnityEngine;

public class NetworkManagerController : MonoBehaviour
{
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 600, 600));
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        GUILayout.EndArea();
    }

    void Start()
    {
    #if UNITY_SERVER
        if (NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.StartServer();
        }
    #endif
    }
}