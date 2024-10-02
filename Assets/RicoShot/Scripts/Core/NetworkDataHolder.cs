using RicoShot.Core;
using RicoShot.Core.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Core
{
    public class NetworkDataHolder : NetworkBehaviour, INetworkDataHolder
    {
        public NetworkList<ClientData> ClientDatas { get; private set; }

        private void Start()
        {
            ClientDatas = new NetworkList<ClientData>();
        }
    }
}
