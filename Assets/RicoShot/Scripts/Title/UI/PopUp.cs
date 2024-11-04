using System;
using DG.Tweening;
using NaughtyAttributes;
using R3;
using RicoShot.Core.Interface;
using RicoShot.Title.Interface;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.Title.UI
{
    public class PopUp : MonoBehaviour
    {
        [Inject] private ITitleSceneManager _titleSceneManager;
        [Inject] private IGameStateManager _gameStateManager;

        [SerializeField] private RectTransform popup;
        [SerializeField] private TMP_Text displayName;
        [SerializeField] private TMP_Text displayNameShadow;
        [SerializeField] private TMP_Text playerProfile;

        [SerializeField] private Button okButton;
        [SerializeField] private Button cancelButton;

        [SerializeField] private Image okButtonOutline;
        [SerializeField] private Image cancelButtonOutline;

        private enum PopUpFocusedOptions
        {
            Ok,
            Cancel
        }

        private PopUpFocusedOptions _popUpFocusedOption;

        [Foldout("Pop Up Settings")] [SerializeField]
        private float duration = 0.3f;

        [Foldout("Pop Up Settings")] [SerializeField]
        private float startY =
            1080;

        [Foldout("Pop Up Settings")] [SerializeField]
        private float endY =
            0;


        private void Awake()
        {
            popup.anchoredPosition = new Vector2(popup.anchoredPosition.x, startY);
        }

        private void Start()
        {
            // TitleState
            Observable.FromEvent<TitleState>(
                    h => _titleSceneManager.OnTitleStateChanged += h,
                    h => _titleSceneManager.OnTitleStateChanged -= h)
                .Subscribe(OnTitleStateChanged).AddTo(this);

            // Confirmed
            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _titleSceneManager.TitleInputs.Main.Confirm.performed += h,
                    h => _titleSceneManager.TitleInputs.Main.Confirm.performed -= h)
                .Subscribe(_ => OnConfirm()).AddTo(this);

            // Focused Left
            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _titleSceneManager.TitleInputs.Main.Left.performed += h,
                    h => _titleSceneManager.TitleInputs.Main.Left.performed -= h)
                .Subscribe(_ => OnFocusLeft()).AddTo(this);

            // Focused Right
            Observable.FromEvent<InputAction.CallbackContext>(
                    h => _titleSceneManager.TitleInputs.Main.Right.performed += h,
                    h => _titleSceneManager.TitleInputs.Main.Right.performed -= h)
                .Subscribe(_ => OnFocusRight()).AddTo(this);
        }

        private void ShowPopup()
        {
            displayName.text = _titleSceneManager.FetchedDisplayName;
            displayNameShadow.text = _titleSceneManager.FetchedDisplayName;
            popup.DOAnchorPos(new Vector2(0, endY), duration).SetEase(Ease.InOutBack);
        }

        private void ClosePopup()
        {
            // Reset Button Selection State to Focus on Ok Button
            OnFocusRight();
            popup.DOAnchorPos(new Vector2(0, startY), duration).SetEase(Ease.InOutBack);
        }

        private void OnConfirm()
        {
            // Do nothing while reading QR Code
            if (_titleSceneManager.TitleState != TitleState.Confirming) return;

            switch (_popUpFocusedOption)
            {
                case PopUpFocusedOptions.Ok:
                    _gameStateManager.NextScene();
                    break;
                case PopUpFocusedOptions.Cancel:
                    _titleSceneManager.TitleState = TitleState.Reading;
                    break;
            }
        }

        private void OnFocusLeft()
        {
            _popUpFocusedOption = PopUpFocusedOptions.Cancel;
            cancelButtonOutline.gameObject.SetActive(true);
            okButtonOutline.gameObject.SetActive(false);
        }

        private void OnFocusRight()
        {
            _popUpFocusedOption = PopUpFocusedOptions.Ok;
            okButtonOutline.gameObject.SetActive(true);
            cancelButtonOutline.gameObject.SetActive(false);
        }

        private void OnTitleStateChanged(TitleState state)
        {
            if (state == TitleState.Confirming) ShowPopup();
            else if (state == TitleState.Reading) ClosePopup();
        }
    }
}