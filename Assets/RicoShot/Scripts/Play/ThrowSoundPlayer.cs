using System.Collections;
using System.Collections.Generic;
using RicoShot.Play.Interface;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace RicoShot.Play
{
    public class ThrowSoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip[] throwSounds;
        
        private LocalPlayerMoveController localPlayerMoveController;
        
        private void Start()
        {
            localPlayerMoveController = GetComponent<LocalPlayerMoveController>();
            localPlayerMoveController.OnFireEvent += PlayThrowSound;
        }

        private void OnDestroy()
        {
            localPlayerMoveController.OnFireEvent -= PlayThrowSound;
        }

        private void PlayThrowSound()
        {
            Debug.Log("PlayThrowSound");
            var sound = throwSounds[Random.Range(0, throwSounds.Length)];
            AudioSource.PlayClipAtPoint(sound, transform.position);
        }
    }
}