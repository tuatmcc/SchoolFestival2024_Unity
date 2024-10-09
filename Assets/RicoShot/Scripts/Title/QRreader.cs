using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;
using ZXing;

namespace RicoShot.Title
{
    public class QRreader : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText; // QRコードの読み取り結果を表示するText
        [SerializeField] private RawImage webCameraRawImage; // Webカメラの映像を表示するRawImage

        private WebCamDevice[] _webCamDevice; // 利用可能なカメラデバイス
        private int _selectingCamera = 0; // 選択中のカメラ

        // UI
        private RawImage _rawImage;
        private WebCamTexture _webCamTexture = null;

        [SerializeField] private Button changeCameraButton;

        private void Start()
        {
            // カメラ切り替えボタンにChangeCameraメソッドを登録
            changeCameraButton.onClick.AddListener(ChangeCamera);
            
            // 利用可能なカメラデバイスを取得
            _webCamDevice = WebCamTexture.devices;

            // Webカメラの映像を取得するために新しいWebCamTextureを作成
            _webCamTexture = new WebCamTexture(_webCamDevice[0].name);

            // Webカメラの映像をスタート
            _webCamTexture.Play();

            // Webカメラの映像をRawImageに表示
            webCameraRawImage.texture = this._webCamTexture;
        }

        private void Update()
        {
            // Webカメラがまだセットされている場合
            if (_webCamTexture != null)
            {
                // Webカメラの映像からQRコードを読み取り、その結果をTextコンポーネントに表示
                resultText.text = Read(this._webCamTexture);
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

            // デコード結果が存在する場合はそのテキストを返し、無ければ空文字を返す
            return "Read val:  " + ((result != null) ? result.Text : string.Empty);
        }

        // カメラの切り替え
        private void ChangeCamera()
        {
            int cameras = _webCamDevice.Length; //カメラの個数
            if (cameras < 1) return; // カメラが1台もなかったら実行せず終了

            // カメラのインデックスをインクリメント
            _selectingCamera++;
            _selectingCamera %= cameras;

            _webCamTexture.Stop(); // カメラを停止
            _webCamTexture = new WebCamTexture(_webCamDevice[_selectingCamera].name); //カメラを変更
            webCameraRawImage.texture = _webCamTexture;
            _webCamTexture.Play(); // 別カメラを開始
        }
    }
}