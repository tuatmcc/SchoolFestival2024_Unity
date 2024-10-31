using Chibi;
using NaughtyAttributes;
using UnityEngine;


namespace RicoShot.Result
{
    // 定義されているメソッドはまだすべて仮
    public class ResultCameraWorkSwitcher : MonoBehaviour
    {
        [SerializeField] private CameraWork[] cameraWorks;
        [SerializeField] private bool debug;

        [EnableIf("debug")] [SerializeField] [Range(0, 3)]
        private int debugCameraIndex;

        private CameraWork _currentCameraWork;

        private CharacterSettingsController _characterSettings;

        private void SelectCameraWork()
        {
            // とりあえずランダムに選択
            var currentIndex = Random.Range(0, cameraWorks.Length);
            // デバッグモードの場合は指定されたカメラを選択
            if (debug) currentIndex = debugCameraIndex;
            foreach (var x in cameraWorks)
                x.gameObject.SetActive(false);
            cameraWorks[currentIndex].gameObject.SetActive(true);
            _currentCameraWork = cameraWorks[currentIndex];
        }

        public void SetCharacterSettings(CharacterSettingsController characterSettings)
        {
            _characterSettings = characterSettings;
        }

        private void Start()
        {
            // これ以前にCharacterSettingsControllerが指定されていることが前提
            SelectCameraWork();
            if (_characterSettings == null)
            {
                Debug.LogError("CharacterSettingsController is not set");
                return;
            }

            _currentCameraWork.CharacterSettings = _characterSettings;
        }
    }
}