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
        public string ServerIPAddress { get => serverIPAddress; set => serverIPAddress = value; }
        public ushort ServerPort { get => serverPort; set => serverPort = value; }
        public string SupabaseURL { get => supabaseURL; set => supabaseURL = value; }
        public string SupabaseSecretKey { get => supabaseSecretkey; set => supabaseSecretkey = value; }
        public int CameraIndex { get => cameraIndex; set => cameraIndex = value; }

        [SerializeField] private string serverIPAddress = "";
        [SerializeField] private ushort serverPort = 7777;
        [SerializeField] private string supabaseURL = "";
        [SerializeField] private string supabaseSecretkey = "";
        [SerializeField] private int cameraIndex = 0;
    }
}
