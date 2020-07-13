using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicArrayLib
{
	public class DynamicArray<T> : AbstractDynamicArray<T>, ICloneable
    {   // Класс DynamicArray, наследуется от AbstractDynamicArray и, по заданию, реализует интерфейс IClonable

        public DynamicArray() : base() { }

        public DynamicArray(int capacity) : base(capacity) { }

        public DynamicArray(IEnumerable<T> collection) : base(collection) { }

        private DynamicArray(T[] collection, int collectionCount)
        {   // Вспомогательный конструктор для метода Clone, принимает базовый массив и длинну (т.к. в случае нашей коллекции длинна != длинне базового списка)
            Count = collectionCount;
            baseArray = new T[collection.Length];
            Array.Copy(collection, baseArray, collection.Length);
        }

        public object Clone()
        {   // Метод прописаный в задании, реализующий интерфейс Clone
            return new DynamicArray<T>(baseArray, Count);
        }


        // Небольшая надстройка над родительским классом

        public override int GetHashCode()
        {   // У стандартных массивов хеш возвращает хеш ссылки, потому, использование baseArray.GetHashCode() не катит
            // Простейшее переопределение GetHashCode - пересчитать хешкод по содержимому массива.

            int sum = 0;
            foreach (T item in baseArray)
            {
                sum += item.GetHashCode();
            }

            return sum;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetHashCode() == GetHashCode())
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(DynamicArray<T> array1, DynamicArray<T> array2)
        {
            return !array1.Equals(array2);
        }

        public static bool operator ==(DynamicArray<T> array1, DynamicArray<T> array2)
        {
            return array1.Equals(array2);
        }

        public static implicit operator DynamicArray<T>(List<T> collection)
        {
            return new DynamicArray<T>(collection);
        }

        public static implicit operator DynamicArray<T>(T[] array)
        {
            return new DynamicArray<T>(array);
        }

        public static implicit operator List<T>(DynamicArray<T> array)
        {
            return array.ToList();
        }

        public static implicit operator T[]( DynamicArray<T> array)
        {
            return  array.ToArray();
        }
    }
}
