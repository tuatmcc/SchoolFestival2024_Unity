using UnityEngine;

namespace Chibi
{
    public class GlassesDisabler : MonoBehaviour
    {
        private CharacterChibiSwitcher _chibiSwitcher;

        private void Awake()
        {
            _chibiSwitcher = GetComponent<CharacterChibiSwitcher>();
        }

        private void Start()
        {
            _chibiSwitcher.current.chibiSettings.accessory = 0;
        }
    }
}