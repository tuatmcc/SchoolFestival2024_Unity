using Chibi;
using R3;
using RicoShot.Core.Interface;
using RicoShot.Title.Interface;
using UnityEngine;
using Zenject;

namespace RicoShot.Title
{
    [RequireComponent(typeof(CharacterSettingsController))]
    public class TitleCharacterInitalizer : MonoBehaviour
    {
        [Inject] private readonly ITitleSceneManager _titleSceneManager;
        [Inject] private readonly ILocalPlayerManager _localPlayerManager;

        private CharacterSettingsController _characterSettingsController;

        private void Start()
        {
            _characterSettingsController = GetComponent<CharacterSettingsController>();
            Observable.FromEvent<TitleState>
                (h => _titleSceneManager.OnTitleStateChanged += h,
                    h => _titleSceneManager.OnTitleStateChanged -= h)
                .Where(state => state == TitleState.Confirming)
                .Subscribe(_ => ReflectCharacterParams()).AddTo(this);
        }

        private void ReflectCharacterParams()
        {
            var characterParams = _localPlayerManager.CharacterParams;
            _characterSettingsController.activeChibiIndex = characterParams.ChibiIndex;
            _characterSettingsController.hairColor = characterParams.HairColor.ToString();
            _characterSettingsController.costumeVariant = characterParams.CostumeVariant;
            _characterSettingsController.accessory = characterParams.Accessory;
        }
    }
}