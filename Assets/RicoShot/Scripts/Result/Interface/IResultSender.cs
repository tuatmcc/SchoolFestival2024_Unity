using System.Collections;
using System.Collections.Generic;
using RicoShot.Core.Interface;
using UnityEngine;
using Zenject;

namespace Ricoshot.Result
{
    public interface IResultSender
    {
        void SendResult();
    }
}