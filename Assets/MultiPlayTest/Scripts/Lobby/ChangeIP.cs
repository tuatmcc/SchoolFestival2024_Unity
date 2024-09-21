using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;  // NGOのUnity Transportを使用するために必要
using Unity.Netcode;  // Netcode for GameObjects

namespace MultiPlayTest.Scripts.Lobby
{
    public class ChangeIP : MonoBehaviour
    {
        [SerializeField] private TMP_InputField ipInputField;
        [SerializeField] private Button applyButton;

        private UnityTransport _transport;

        private void Start()
        {
            // UnityTransportコンポーネントを取得
            _transport = FindObjectOfType<UnityTransport>();

            // ボタンを押すとIPアドレスを変更する
            applyButton.onClick.AddListener(OnApplyButtonClicked);
        }

        private void OnApplyButtonClicked()
        {
            // テキストフィールドからIPアドレスを取得
            string newIPAddress = ipInputField.text;

            if (IsValidIPAddress(newIPAddress))
            {
                // UnityTransportのIPアドレスを変更
                _transport.ConnectionData.Address = newIPAddress;
                Debug.Log($"IPアドレスが {newIPAddress} に変更されました");
            }
            else
            {
                Debug.LogWarning("無効なIPアドレスです");
            }
        }

        // IPアドレスが正しいか確認する簡単なバリデーション
        private bool IsValidIPAddress(string ipAddress)
        {
            System.Net.IPAddress address;
            return System.Net.IPAddress.TryParse(ipAddress, out address);
        }
    }
}