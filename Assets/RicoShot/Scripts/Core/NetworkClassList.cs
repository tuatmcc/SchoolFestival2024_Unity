using RicoShot.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace RicoShot.Core
{
    /// <summary>
    /// Netcodeでクラスのリストを同期するためにNetworkVariableBaseとIListを実装したクラス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [GenerateSerializationForGenericParameter(0)]
    public class NetworkClassList<T> : NetworkVariableBase, IList<T> where T : IDataChangedNotifiable
    {
        public event Action OnDataChanged;

        public List<T> SomeDataToSynchronize { get; private set; } = new List<T>();
        public int Count {  get { return SomeDataToSynchronize.Count; } }

        public bool IsReadOnly => false;

        public T this[int index] { get => SomeDataToSynchronize[index]; set => SomeDataToSynchronize[index] = value; }

        public override void WriteField(FastBufferWriter writer)
        {
            writer.WriteValueSafe(SomeDataToSynchronize.Count);
            for (int i = 0; i < SomeDataToSynchronize.Count; i++)
            {
                var dataEntry = SomeDataToSynchronize[i];
                NetworkVariableSerialization<T>.Write(writer, ref dataEntry);
            }
        }

        public override void ReadField(FastBufferReader reader)
        {
            reader.ReadValueSafe(out int itemsToUpdate);
            SomeDataToSynchronize.Clear();
            for (int i = 0; i < itemsToUpdate; i++)
            {
                T newEntry = default;
                NetworkVariableSerialization<T>.Read(reader, ref newEntry);
                SomeDataToSynchronize.Add(newEntry);
            }
        }

        public override void WriteDelta(FastBufferWriter writer)
        {
            WriteField(writer);
        }

        public override void ReadDelta(FastBufferReader reader, bool keepDirtyDelta)
        {
            ReadField(reader);
            Debug.Log("Data received");
            OnDataChanged?.Invoke();
        }

        public override void SetDirty(bool isDirty)
        {
            base.SetDirty(isDirty);
            OnDataChanged?.Invoke();
        }

        // 各要素のOnDataChangedイベントに登録するための関数
        private void ElementDataChanged()
        {
            SetDirty(true);
        }

        public int IndexOf(T item)
        {
            return SomeDataToSynchronize.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            SomeDataToSynchronize.Insert(index, item);
            SetDirty(true);
        }

        public void RemoveAt(int index)
        {
            SomeDataToSynchronize.RemoveAt(index);
            SetDirty(true);
        }

        public void Add(T item)
        {
            SomeDataToSynchronize.Add(item);
            SetDirty(true);
            item.OnDataChanged += ElementDataChanged;
        }

        public void Clear()
        {
            SomeDataToSynchronize.Clear();
            SetDirty(true);
        }

        public bool Contains(T item)
        {
            return SomeDataToSynchronize.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            SetDirty(true);
            return SomeDataToSynchronize.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return SomeDataToSynchronize.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}