using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;
using RicoShot.ModeSelect;

namespace RicoShot {
    public class GameConfigUIManager : MonoBehaviour
    {
        [Inject] private RicoShot.Core.Interface.IGameStateManager gameStateManager;
        [Inject] private ModeSelectSceneManager modeSelectSceneManager;

        [SerializeField]
        public TMP_InputField InputField_ServerIPAddress;
        public TMP_InputField InputField_ServerPort;
        public TMP_InputField InputField_SupabaseURL;
        public TMP_InputField InputField_SupabaseSecretKey;
        public TMP_InputField InputField_CameraIndex;

        // Start is called before the first frame update
        void Start()
        {
            InputField_ServerIPAddress.text = gameStateManager.GameConfig.ServerIPAddress.ToString();
            InputField_ServerPort.text = gameStateManager.GameConfig.ServerPort.ToString();
            InputField_SupabaseURL.text = gameStateManager.GameConfig.SupabaseURL.ToString();
            InputField_SupabaseSecretKey.text = gameStateManager.GameConfig.SupabaseSecretKey.ToString();
            InputField_CameraIndex.text = gameStateManager.GameConfig.CameraIndex.ToString();
            
            InputField_ServerIPAddress.onSelect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Disable());
            InputField_ServerPort.onSelect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Disable());
            InputField_SupabaseURL.onSelect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Disable());
            InputField_SupabaseSecretKey.onSelect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Disable());
            InputField_CameraIndex.onSelect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Disable());
            
            InputField_ServerIPAddress.onDeselect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Enable());
            InputField_ServerPort.onDeselect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Enable());
            InputField_SupabaseURL.onDeselect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Enable());
            InputField_SupabaseSecretKey.onDeselect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Enable());
            InputField_CameraIndex.onDeselect.AddListener(_ => modeSelectSceneManager.ModeSeletotInputs.Enable());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Check_ServeIPAddr()
        {
            gameStateManager.GameConfig.ServerIPAddress = InputField_ServerIPAddress.text;
        }

        public void Check_Port()
        {
            if(ushort.TryParse(InputField_ServerPort.text,out ushort port))
            {
                gameStateManager.GameConfig.ServerPort = port;
            }
            else
            {
                Debug.Log("Invalid Value: " + InputField_ServerPort.text);
                InputField_ServerPort.text = "";
            }
        }
        public void Check_URL()
        {
            if (IsUrl(InputField_SupabaseURL.text))
            {
                gameStateManager.GameConfig.SupabaseURL = InputField_SupabaseURL.text;
            }
            else
            {
                Debug.Log("Invalid Value: " + InputField_SupabaseURL.text);
                InputField_SupabaseURL.text = "";
            }
        }

        public static bool IsUrl(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(
               input,
               @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"
            );
        }

        public void Check_SecretKey()
        {
            gameStateManager.GameConfig.SupabaseSecretKey = InputField_SupabaseSecretKey.text;
        }

        public void Check_CameraIndex()
        {
            if(int.TryParse(InputField_CameraIndex.text,out int idx))
            {
                gameStateManager.GameConfig.CameraIndex = idx;
            }
            else
            {
                InputField_CameraIndex.text = "";
            }
        }
    }
}

