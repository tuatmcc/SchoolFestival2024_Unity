using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RicoShot.Play.Interface
{
    public interface ITimeManager
    {
        public event Action<int> OnCountChanged;
        public event Action<float> OnPlayTimeChanged;

        public int Count { get; }
        public float PlayTime { get; }
    }
}
