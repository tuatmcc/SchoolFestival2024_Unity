using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using ZXing;
using Zenject;
using RicoShot.Core;
using RicoShot.Core.Interface;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using RicoShot.Title.Interface;

namespace RicoShot.Title
{
    public class QRReader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText; // QRコードの読み取り結果を表示するText
        [SerializeField] private RawImage webCameraRawImage; // Webカメラの映像を表示するRawImage

        private WebCamDevice[] _webCamDeviceList; // 利用可能なカメラデバイス
        private int _selectingCameraIndex = 0; // 選択中のカメラ

        // UI
        private RawImage _rawImage;
        private WebCamTexture _webCamTexture = null;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ITitleSceneManager titleSceneManager;

        private void Start()
        {
            // カメラ変更のeventを登録
            titleSceneManager.TitleInputs.Main.Right.performed += CameraIndexIncrement;
            titleSceneManager.TitleInputs.Main.Left.performed += CameraIndexDecrement;

            // 利用可能なカメラデバイスを取得
            _webCamDeviceList = WebCamTexture.devices;
            _selectingCameraIndex = gameStateManager.GameConfig.GetCameraIndex();

            // 例外処理
            if (_webCamDeviceList.Length == 0)
            {
                Debug.LogWarning("There are no camera.");
            }
            if (!(0 <= _selectingCameraIndex && _selectingCameraIndex < _webCamDeviceList.Length))
            {
                Debug.Log($"Camera index: {_webCamDeviceList} is invalid. Automatically, set camera index 0");
                _selectingCameraIndex = 0;
                gameStateManager.GameConfig.SetCameraIndex(0);
            }

            // Webカメラの映像を取得するために新しいWebCamTextureを作成
            _webCamTexture = new WebCamTexture(_webCamDeviceList[_selectingCameraIndex].name);

            // Webカメラの映像をスタート
            _webCamTexture.Play();

            // Webカメラの映像をRawImageに表示
            webCameraRawImage.texture = this._webCamTexture;

            Debug.Log($"Camera started index: {_selectingCameraIndex}");
        }

        private void Update()
        {
            // Webカメラがまだセットされている場合
            if (_webCamTexture != null)
            {
                titleSceneManager.TitleState = TitleState.Read;

                // Webカメラの映像からQRコードを読み取り、その結果をTextコンポーネントに表示
                resultText.text = $"Read: {Read(this._webCamTexture)}";
            }
            else
            {
                Debug.Log("WebCamTexture is null");
            }
        }

        // QRコードの読み取り処理
        private static string Read(WebCamTexture texture)
        {
            // ZXingのBarcodeReaderクラスを使用してQRコードをデコード
            BarcodeReader reader = new BarcodeReader();

            // Webカメラの映像をピクセルデータとして取得
            Color32[] rawRGB = texture.GetPixels32();

            // Webカメラの映像の幅と高さを取得
            int width = texture.width;
            int height = texture.height;

            // ピクセルデータからQRコードをデコード
            var result = reader.Decode(rawRGB, width, height);

            // デコード結果が存在する場合はそのテキストを返し、無ければnullを返す
            return result?.Text;
        }

        private void CameraIndexIncrement(InputAction.CallbackContext context)
        {
            ChangeCamera(1);
        }

        private void CameraIndexDecrement(InputAction.CallbackContext context)
        {
            ChangeCamera(-1);
        }

        // カメラの切り替え
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ChangeCamera(int delta)
        {
            int cameras = _webCamDeviceList.Length; //カメラの個数
            if (cameras < 1) return; // カメラが1台もなかったら実行せず終了

            // カメラのインデックスをインクリメント
            _selectingCameraIndex += (delta + cameras);
            _selectingCameraIndex %= cameras;

            _webCamTexture.Stop(); // カメラを停止
            _webCamTexture = new WebCamTexture(_webCamDeviceList[_selectingCameraIndex].name); //カメラを変更
            webCameraRawImage.texture = _webCamTexture;
            _webCamTexture.Play(); // 別カメラを開始

            Debug.Log($"Camera changed to index: {_selectingCameraIndex}");
        }

        private void OnDestroy()
        {
            gameStateManager.GameConfig.SetCameraIndex(_selectingCameraIndex);
            if (_webCamTexture != null)
            {
                _webCamTexture.Stop();
            }
        }
    }
}