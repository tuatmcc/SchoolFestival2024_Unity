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
using Cysharp.Threading.Tasks;
using R3;

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
        private string readString;

        [Inject] private readonly IGameStateManager gameStateManager;
        [Inject] private readonly ITitleSceneManager titleSceneManager;

        private void Start()
        {
            // カメラ変更のeventを登録
            Observable.FromEvent<InputAction.CallbackContext>
                (h => titleSceneManager.TitleInputs.Main.Select.performed += h,
                    h => titleSceneManager.TitleInputs.Main.Select.performed -= h)
                .Subscribe(_ => CameraIndexIncrement()).AddTo(this);


            Debug.Log($"Camera started index: {_selectingCameraIndex}");

            StartCamera();

            // 0.5秒ごとにQRコードを読み取る
            Observable.Interval(TimeSpan.FromSeconds(0.5f)).Subscribe(_ =>
            {
                if (titleSceneManager.TitleState != TitleState.Reading) return;

                if (_webCamTexture == null) return;
                var result = Read(_webCamTexture);
                if (result == null) return;
                titleSceneManager.FetchData(result);
                readString = null;
            }).AddTo(this);
        }

        private void StartCamera()
        {
            // 利用可能なカメラデバイスを取得
            _webCamDeviceList = WebCamTexture.devices;
            _selectingCameraIndex = gameStateManager.GameConfig.CameraIndex;

            if (_webCamDeviceList.Length == 0) Debug.LogWarning("There are no camera.");

            if (!(0 <= _selectingCameraIndex && _selectingCameraIndex < _webCamDeviceList.Length))
            {
                Debug.Log($"Camera index: {_webCamDeviceList} is invalid. Automatically, set camera index: 0");
                _selectingCameraIndex = 0;
                gameStateManager.GameConfig.CameraIndex = 0;
            }

            // Webカメラの映像を取得するために新しいWebCamTextureを作成
            _webCamTexture = new WebCamTexture(_webCamDeviceList[_selectingCameraIndex].name);

            // Webカメラの映像をスタート
            _webCamTexture.Play();

            // Webカメラの映像をRawImageに表示
            webCameraRawImage.texture = _webCamTexture;
        }

        // QRコードの読み取り処理
        private static string Read(WebCamTexture texture)
        {
            // ZXingのBarcodeReaderクラスを使用してQRコードをデコード
            var reader = new BarcodeReader();

            // Webカメラの映像をピクセルデータとして取得
            var rawRGB = texture.GetPixels32();

            // Webカメラの映像の幅と高さを取得
            var width = texture.width;
            var height = texture.height;

            // ピクセルデータからQRコードをデコード
            var result = reader.Decode(rawRGB, width, height);

            // デコード結果が存在する場合はそのテキストを返し、無ければnullを返す
            return result?.Text;
        }

        private void CameraIndexIncrement()
        {
            ChangeCamera(1);
        }

        // カメラの切り替え
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void ChangeCamera(int delta)
        {
            var cameras = _webCamDeviceList.Length; //カメラの個数
            if (cameras < 1) return; // カメラが1台もなかったら実行せず終了

            // カメラのインデックスをインクリメント
            _selectingCameraIndex += delta + cameras;
            _selectingCameraIndex %= cameras;

            _webCamTexture.Stop(); // カメラを停止
            _webCamTexture = new WebCamTexture(_webCamDeviceList[_selectingCameraIndex].name); //カメラを変更
            webCameraRawImage.texture = _webCamTexture;
            _webCamTexture.Play(); // 別カメラを開始

            Debug.Log($"Camera changed to index: {_selectingCameraIndex}");
        }

        private void OnDestroy()
        {
            gameStateManager.GameConfig.CameraIndex = _selectingCameraIndex;
            if (_webCamTexture != null) _webCamTexture.Stop();
        }
    }
}