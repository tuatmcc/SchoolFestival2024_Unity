using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Netcode;

namespace RicoShot.Core
{
    [Serializable]
    [GenerateSerializationForGenericParameter(0)]
    public class NetworkClassList<T> : NetworkVariableBase, IList<T>
    {
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
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            SomeDataToSynchronize.Add(item);
        }

        public void Clear()
        {
            SomeDataToSynchronize.Clear();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
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