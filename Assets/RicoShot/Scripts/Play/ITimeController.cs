using System;

namespace RicoShot.Play
{
    public interface ITimeController
    {
        static int TIME_LIMIT = 60;

        event Action<int> OnTimeChangedSeconds;
        event Action OnTimeOver;
    }
}