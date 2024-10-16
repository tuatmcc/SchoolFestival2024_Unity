using System;

namespace RicoShot.Play
{
    public interface ITimeController
    {
        static int TIME_LIMIT = 180;

        event Action<int> OnTimeChangedSeconds;
        event Action OnTimeOver;
    }
}