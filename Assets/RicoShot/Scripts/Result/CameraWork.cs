﻿using Chibi;
using UnityEngine;

namespace RicoShot.Result
{
    public class CameraWork : MonoBehaviour
    {
        [SerializeField] private CharacterSettingsController characterSettings;

        public CharacterSettingsController CharacterSettings => characterSettings;
    }
}