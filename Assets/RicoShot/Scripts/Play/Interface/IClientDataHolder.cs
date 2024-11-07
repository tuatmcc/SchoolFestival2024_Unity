using RicoShot.Core;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface IClientDataHolder
    {
        public ClientData ClientData { get; }
        public Vector3 SpawnPosition { get; }
        public Quaternion SpawnRotation { get; }
    }
}
