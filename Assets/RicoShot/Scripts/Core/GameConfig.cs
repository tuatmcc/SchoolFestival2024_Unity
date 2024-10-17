using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Core
{
    [Serializable]
    public class GameConfig
    {
        [SerializeField] private string SupabaseURL = "";
        [SerializeField] private string SupabaseSecretkey = "";
        [SerializeField] private int CameraIndex = 0;

        public string GetSupabaseURL() { return SupabaseURL; }
        public string GetSupabaseSecretkey() { return SupabaseSecretkey; }

        public void SetCameraIndex(int cameraIndex) { CameraIndex = cameraIndex; }

        public int GetCameraIndex() { return CameraIndex; }
    }
}
