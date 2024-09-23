using RicoShot.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RicoShot.ModeSelect
{
    [RequireComponent(typeof(Button))]
    public class ModeSelectButton : MonoBehaviour
    {
        [SerializeField] private NetworkMode networkMode;

        [Inject] private ModeSelectSceneManager modeSelectSceneManager;

        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(() => { modeSelectSceneManager.SetNetworkMode(networkMode); });
        }

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners(); 
        }
    }
}
