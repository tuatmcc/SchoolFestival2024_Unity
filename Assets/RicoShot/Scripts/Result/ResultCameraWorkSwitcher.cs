using Chibi;
using Chibi.ChibiComponents;
using NaughtyAttributes;
using RicoShot.Core;
using UnityEngine;
using Random = UnityEngine.Random;


namespace RicoShot.Result
{
    // 定義されているメソッドはまだすべて仮
    // TODO: Result Scene Manager を実装してデバッグ機能をそちらへ移す
    public class ResultCameraWorkSwitcher : MonoBehaviour
    {
        public const int CameraWorkMaxIndex = 3;
        [SerializeField] private CameraWork[] cameraWorks;
        [SerializeField] private bool debug;

        [EnableIf("debug")] [SerializeField] [Range(0, CameraWorkMaxIndex)]
        private int debugCameraWorkIndex;

        [EnableIf("debug")] [SerializeField] [Range(0, CharacterSettingsController.MAX_CHIBI_INDEX)]
        private int debugChibiIndex;

        [EnableIf("debug")] [SerializeField] [Range(0, ChibiCostumeColorSettings.MAX_COSTUME_VARIANT_INDEX)]
        private int debugCostumeVariant;

        [EnableIf("debug")] [SerializeField] private string debugHairColor = "#000000";

        private CameraWork _currentCameraWork;

        public CharacterParams CharacterParams { get; private set; }

        private void Awake()
        {
            CharacterParams = new CharacterParams(debugChibiIndex, debugHairColor, debugCostumeVariant, 0);
        }

        private void SelectCameraWork()
        {
            // とりあえずランダムに選択
            var currentIndex = Random.Range(0, CameraWorkMaxIndex + 1);
            // デバッグモードの場合は指定されたカメラを選択
            if (debug) currentIndex = debugCameraWorkIndex;
            foreach (var x in cameraWorks)
                x.gameObject.SetActive(false);
            cameraWorks[currentIndex].gameObject.SetActive(true);
            _currentCameraWork = cameraWorks[currentIndex];
        }

        public void SetCharacterParams(CharacterParams characterParams)
        {
            CharacterParams = characterParams;
        }

        private void Start()
        {
            // これ以前にCharacterSettingsControllerが指定されていることが前提
            SelectCameraWork();

            if (debug)
            {
            }
        }

        private void OnValidate()
        {
            if (CameraWorkMaxIndex != cameraWorks.Length - 1)
            {
                Debug.LogError("CameraWorkMaxIndexとcameraWorksの要素数が一致しません");
            }
        }
    }
}