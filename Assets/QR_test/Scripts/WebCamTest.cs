// Webカメラの映像を表示するテスト用のスクリプト
// 使用はしていない

using UnityEngine;
using UnityEngine.UI;

namespace QR_test.Scripts
{
    public class WebCamTest : MonoBehaviour
    {
        private const int Height = 1920;
        private const int Width = 1080;
        private const int FPS = 30;

        // UI
        private RawImage _rawImage;
        private WebCamTexture _webCamTexture;

        void Start()
        {
            // Webカメラの開始
            this._rawImage = GetComponent<RawImage>();
            this._webCamTexture = new WebCamTexture(Width, Height, FPS);
            this._rawImage.texture = this._webCamTexture;
            this._webCamTexture.Play();
        }
    }
}