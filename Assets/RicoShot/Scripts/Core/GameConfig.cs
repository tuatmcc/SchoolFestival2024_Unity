using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Core
{
    /// <summary>
    /// ゲーム内の設定を保持するクラス
    /// </summary>
    [Serializable]
    public class GameConfig
    {
        public string ServerIPAddress 
        {
            get
            {
                if (serverIPAddress == string.Empty)
                {
                    serverIPAddress = DefaultServerIPAddress;
                }
                return serverIPAddress;
            }
            set => serverIPAddress = value;
        }
        public ushort ServerPort
        {
            get
            {
                if (serverPort == 0)
                {
                    serverPort = DefaultServerPort;
                }
                return serverPort;
            }
            set => serverPort = value;
        }
        public string SupabaseURL { get => supabaseURL; set => supabaseURL = value; }
        public string SupabaseSecretKey { get => supabaseSecretkey; set => supabaseSecretkey = value; }
        public int CameraIndex { get => cameraIndex; set => cameraIndex = value; }

        [SerializeField] private string serverIPAddress = DefaultServerIPAddress;
        [SerializeField] private ushort serverPort = DefaultServerPort;
        [SerializeField] private string supabaseURL = "";
        [SerializeField] private string supabaseSecretkey = "";
        [SerializeField] private int cameraIndex = 0;

        private const string DefaultServerIPAddress = "127.0.0.1";
        private const int DefaultServerPort = 7777;
    }
}
