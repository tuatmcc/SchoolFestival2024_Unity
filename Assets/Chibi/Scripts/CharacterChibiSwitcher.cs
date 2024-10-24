using UnityEngine;

namespace Chibi
{
    public class CharacterChibiSwitcher : MonoBehaviour
    {
        [SerializeField] private Chibi[] _chibis;

        private int _currentChibiIndex;

        public Chibi[] chibis => _chibis;

        public Chibi current => _chibis[_currentChibiIndex];

        public int currentChibiIndex
        {
            get => _currentChibiIndex;
            set
            {
                if (value < 0 || value >= _chibis.Length)
                {
                    Debug.LogError("Invalid chibi index");
                    return;
                }

                _currentChibiIndex = value;

                for (var i = 0; i < _chibis.Length; i++) _chibis[i].gameObject.SetActive(i == value);
            }
        }
    }
}