using RicoShot.Core;
using UnityEngine;
using Zenject;

namespace RicoShot {
    public class ServerPlayerCamController : MonoBehaviour
    {
        [Inject] private readonly GameStateManager gameStateManager;
        // Start is called before the first frame update
        void Start()
        {
            if(gameStateManager.NetworkMode == NetworkMode.Client)
            {
                this.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}