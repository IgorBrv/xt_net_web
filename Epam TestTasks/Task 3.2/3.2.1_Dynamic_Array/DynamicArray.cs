using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamic_Array
{
    public class DynamicArray<T> : IList<T>, ICloneable
    {
        private T[] baseArray;
        private int count = 0;
        private bool readOnly = false;

        public DynamicArray()
        {
            baseArray = new T[8];
            Console.WriteLine(baseArray.Length);
        }

        public DynamicArray(int capacity)
        {
            baseArray = new T[capacity];
            Console.WriteLine(baseArray.Length);
        }

        public DynamicArray(IEnumerable<T> collection)
        {
            baseArray = collection.ToArray();
            this.count = collection.Count();
        }

        private DynamicArray(T[] collection, int count)
        {
            baseArray = new T[collection.Length];
            Array.Copy(collection, baseArray, collection.Length);
            this.count = count;
        }

        public T this[int index]
        {
            get
            {
                if (count != 0 && index < count)
                {
                    return baseArray[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                baseArray[index] = value;
            }
        }

        public int Count => count;

        public bool IsReadOnly => readOnly;

        public void Add(T item)
        {
            if (count >= baseArray.Length)
            {
                T[] tempArray = new T[baseArray.Length*2];
                for (int i = 0; i < count; i++)
                {
                    tempArray[i] = baseArray[i];
                }
                baseArray = tempArray;
            }
            baseArray[count] = item;
            count++;
        }

        public void Clear()
        {
            baseArray = new T[8];
            count = 0;
        }

        public object Clone()
        {
            return new DynamicArray<T>(baseArray, count);
        }


        public bool Contains(T item)
        {
            return baseArray.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)baseArray.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
