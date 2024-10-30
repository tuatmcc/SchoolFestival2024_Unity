using System;
using UnityEngine;

namespace Chibi
{
    [RequireComponent(typeof(CharacterSettingsController))]
    public class CharacterAnimatiorSettings : MonoBehaviour
    {
        private Animator _animator;
        private CharacterSettingsController _settingsController;

        private void Awake()
        {
            _settingsController = GetComponent<CharacterSettingsController>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _animator.avatar = _settingsController.animator.avatar;
        }
    }
}