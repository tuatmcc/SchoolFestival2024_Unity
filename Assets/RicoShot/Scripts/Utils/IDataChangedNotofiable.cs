using System;

namespace RicoShot.Utils
{
    public interface IDataChangedNotofiable
    {
        event Action OnDataChanged;
    }
}
