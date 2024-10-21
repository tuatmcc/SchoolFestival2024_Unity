using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Core
{
    [Serializable]
    public class GameConfig
    {
        public string ServerIPAddress { get => serverIPAddress; set => serverIPAddress = value; }
        public string SupabaseURL { get => supabaseURL; set => supabaseURL = value; }
        public string SupabaseSecretKey { get => supabaseSecretkey; set => supabaseSecretkey = value; }
        public int CameraIndex { get => cameraIndex; set => cameraIndex = value; }

        [SerializeField] private string serverIPAddress = "";
        [SerializeField] private string supabaseURL = "";
        [SerializeField] private string supabaseSecretkey = "";
        [SerializeField] private int cameraIndex = 0;
    }
}
