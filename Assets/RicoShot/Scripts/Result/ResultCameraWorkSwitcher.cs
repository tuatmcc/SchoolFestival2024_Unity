using RicoShot.Core.Interface;
using RicoShot.Result.Interface;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace RicoShot.Result
{
    // 定義されているメソッドはまだすべて仮
    // TODO: Result Scene Manager を実装してデバッグ機能をそちらへ移す
    public class ResultCameraWorkSwitcher : MonoBehaviour
    {
        public const int CameraWorkMaxIndex = 3;

        [Inject] private IResultSceneManager _resultSceneManager;
        [Inject] private IResultSceneTester _resultSceneTester;
        [Inject] private ILocalPlayerManager _localPlayerManager;

        [SerializeField] private CameraWork[] cameraWorks;

        private CameraWork _cameraWork;

        private void SelectCameraWork()
        {
            // とりあえずランダムに選択
            var currentIndex = _resultSceneTester.TestEnabled
                ? _resultSceneTester.CameraWorkIndex
                : Random.Range(0, CameraWorkMaxIndex + 1);

            // デバッグモードの場合は指定されたカメラを選択
            foreach (var x in cameraWorks)
                x.gameObject.SetActive(false);
            cameraWorks[currentIndex].gameObject.SetActive(true);
            _cameraWork = cameraWorks[currentIndex];
        }

        private void SetupCharacterSettings()
        {
            var characterParams = _resultSceneTester.TestEnabled
                ? _resultSceneTester.CharacterParams
                : _resultSceneManager.CharacterParams;
            _cameraWork.CharacterSettings.activeChibiIndex = characterParams.ChibiIndex;
            _cameraWork.CharacterSettings.accessory = characterParams.Accessory;
            _cameraWork.CharacterSettings.hairColor = characterParams.HairColor.ToString();
            _cameraWork.CharacterSettings.costumeVariant = characterParams.CostumeVariant;
        }

        private void Start()
        {
            SelectCameraWork();
            SetupCharacterSettings();
        }

        private void OnValidate()
        {
            if (CameraWorkMaxIndex != cameraWorks.Length - 1)
                Debug.LogError("CameraWorkMaxIndexとcameraWorksの要素数が一致しません");
        }
    }
}