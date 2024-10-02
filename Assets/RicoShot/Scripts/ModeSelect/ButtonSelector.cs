using RicoShot.InputActions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.ModeSelect
{
    public class ButtonSelector : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        [Inject] private ModeSelectSceneManager modeSelectSceneManager;

        private ModeSelectInputs inputs;

        private void Start()
        {
            inputs = modeSelectSceneManager.ModeSeletotInputs;
            inputs.Select.SelectRight.performed += SelectRight;
            inputs.Select.SelectLeft.performed += SelectLeft;
        }

        private void SelectRight(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            EventSystem.current.SetSelectedGameObject(leftButton.gameObject);
        }

        private void SelectLeft(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            EventSystem.current.SetSelectedGameObject(rightButton.gameObject);
        }
    }
}
