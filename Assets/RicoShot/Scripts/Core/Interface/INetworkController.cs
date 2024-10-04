using Unity.Netcode;

namespace RicoShot.Core.Interface
{
    public interface INetworkController
    {
        public NetworkList<ClientData> ClientDatas { get; }
    }
}
