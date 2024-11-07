using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface ICharacterGenerator
    {
        public List<Transform> PlayerTransforms { get; }
    }
}