using System;

namespace RicoShot.Utils
{
    /// <summary>
    /// NetworkClassListの要素が実装すべきinterface
    /// データの変更を通知するeventを実装する
    /// </summary>
    public interface IDataChangedNotifiable
    {
        event Action OnDataChanged;
    }
}
