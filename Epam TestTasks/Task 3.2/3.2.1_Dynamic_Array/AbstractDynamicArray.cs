using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArrayLib
{   // На базе массива (именно массива, специфичные коллекции .NET не использовать) реализовать свой собственный класс DynamicArray<T>,
    // представляющий собой массив с запасом, хранящий объекты произвольных типов. 

    public abstract class AbstractDynamicArray<T> : IList<T>
    {   // Абстрактный класс динамического массива. Реализует интерфейс IList.
        protected T[] baseArray;
        protected int defaultArrayLength = 8;   // По заданию, по дефолту создаётся коллекция с внутренней вместимостью 8 элементов


        public AbstractDynamicArray()
        {   // Базовый конструктор
            baseArray = new T[defaultArrayLength];
        }

        public AbstractDynamicArray(int capacity)
        {   // Конструктор определенный заданием, создаёт новую коллекцию DynamicArray заданной базовой вместимости
            defaultArrayLength = capacity;
            baseArray = new T[defaultArrayLength];
        }

        public AbstractDynamicArray(IEnumerable<T> collection)
        {   // Конструктор определенный заданием, создаёт новую коллекцию DynamicArray из любой коллекции реализующей IEnumerable
            Count = collection.Count();
            baseArray = collection.ToArray();
        }


        public bool IsReadOnly { get; set; } = false;   // Наличия IsReadOnly требует реализация интерфейса IList. В данной коллекции реализую проверку IsReadonly для всех методов изменяющих массив.

        public int Capacity => baseArray.Length;    // Свойство отображающее вместимость внутреннего массива

        public int Count { get; set; } = 0;     // Отображает длинну (заполнение) коллекции

        public T this[int index]
        {   // Доступ к элементам коллекции по индексу
            get
            {
                if (Count != 0 && index < Count && index >= -Count)
                {
                    if (index < 0)
                    {   // Реализация доступа по отрицательному индексу (По заданию)
                        index = Count + index;
                    }

                    return baseArray[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Argument is out of range");
                }
            }
            set
            {
                if (!IsReadOnly)
                {
                    if (Count != 0 && index < Count && index >= -Count)
                    {
                        if (index < 0)
                        {   // Реализация доступа по отрицательному индексу (По заданию)
                            index = Count - index;
                        }

                        baseArray[index] = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("Argument is out of range");
                    }
                }
                else throw new ReadOnlyException("list has readonly:true flag");
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {   // Метод копирующий элементы коллекции в массив по индексу
            if (array.Length - arrayIndex >= Count)
            {
                try
                {
                    baseArray.CopyTo(array, arrayIndex);
                }
                catch (RankException ex)
                {   // Проброс исключений, который, в данном случае, скорее всего, никогда не произойдёт, исключительно для закрепления пройденного материала
                    throw new RankException("Rank mismatch", ex);
                }
                catch (ArrayTypeMismatchException ex)
                {   // Проброс исключений, который, в данном случае, скорее всего, никогда не произойдёт, исключительно для закрепления пройденного материала
                    throw new ArrayTypeMismatchException("Array Type Mismatch", ex);
                }
                catch (ArgumentNullException ex)
                {   // Проброс исключений, который, в данном случае, скорее всего, никогда не произойдёт, исключительно для закрепления пройденного материала
                    throw new ArgumentException("Argument cant be null", ex);
                }
                catch (ArgumentException ex)
                {   // Проброс исключений, который, в данном случае, скорее всего, никогда не произойдёт, исключительно для закрепления пройденного материала
                    throw new ArgumentException("One of argumets is unexpectable", ex);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Length of destination array is not enouth!");
            }

        }

        public int IndexOf(T item)
        {   // Метод, возвращающий индекс первого вхождения запрашиваемого элемента
            for (int i = 0; i < Count; i++)
            {
                if (baseArray[i].GetHashCode() == item.GetHashCode())
                {
                    return i;
                }
            }

            return -1;
        }

        public bool Contains(T item)
        {   // Метод возвращающий индекс вхождения элемента в массив
            return baseArray.Contains(item);
        }

        //---------Секция методов производящих действия с внутренним массивом и проверяющих свойство isReadOnly------------

        public void Insert(int index, T item)
        {   // Метод вставляющий элемент в коллекцию по заданному индексу
            if (!IsReadOnly)
            {
                if (baseArray.Length == Count)
                {
                    ChangeArraySize(x => x * 2);
                }

                for (int i = Count; i > index; i--)
                {
                    baseArray[i] = baseArray[i - 1];
                }

                baseArray[index] = item;
                Count++;
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }


        public void Add(T item)
        {   // Метод добавляющий элемент в конец коллекции
            if (!IsReadOnly)
            {
                if (Count >= baseArray.Length)  // В случае, если длины массива не достаточно создаётся новый массив удвоенной длины
                {
                    ChangeArraySize(x => x * 2);
                }
                baseArray[Count] = item;
                Count++;
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }

        public void AddRange(IEnumerable<T> collection)
        {   // Метод, копирующий содержимое входящей коллекции реализующей IEnumerable в конец данной коллекции
            if (!IsReadOnly)
            {
                if (baseArray.Length - Count < collection.Count())
                {   // Увеличиваем размер внутреннего массива, если длины не хватит для вмещения входящей коллекции

                    int slotsToBeAdded = collection.Count() - (baseArray.Length - Count);
                    ChangeArraySize(x => x + slotsToBeAdded);
                }

                collection.ToArray().CopyTo(baseArray, Count);
                Count += collection.Count();
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }

        public bool Remove(T item)
        {   // Метод удаляющий элемент коллекции по первому вхождению.
            if (!IsReadOnly)
            {
                if (baseArray.Contains(item))
                {
                    bool isRemoved = false;
                    for (int i = 0; i < Count; i++)
                    {
                        if (isRemoved)
                        {
                            baseArray[i] = baseArray[i + 1];
                        }
                        else
                        {
                            if (baseArray[i].GetHashCode() == item.GetHashCode())
                            {
                                baseArray[i] = baseArray[i + 1];
                                isRemoved = true;
                                Count--;
                            }
                        }
                    }

                    if (baseArray.Length > Count * 2 && baseArray.Length > defaultArrayLength)
                    {
                        ChangeArraySize(x => x / 2);
                    }

                    return true;
                }
                else return false;
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }

        public void RemoveAt(int index)
        {   // Метод удаляющий элемент коллекции по индексу
            if (!IsReadOnly)
            {
                for (int i = index; i < Count; i++)
                {
                    baseArray[i] = baseArray[i + 1];
                }
                Count--;
                if (baseArray.Length > Count * 2 && baseArray.Length > defaultArrayLength)
                {
                    ChangeArraySize(x => x / 2);
                }
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }

        public void Clear()
        {   // Метод производящий очистку массива до базового состояния
            if (!IsReadOnly)
            {
                baseArray = new T[defaultArrayLength];
                Count = 0;
            }
            else throw new ReadOnlyException("list has readonly:true flag");
        }

        // -----------------------------------------------------------------------------------------

        public virtual IEnumerator<T> GetEnumerator()
        {   // Виртуальный Ienumerator, предполагает назначение в классе-наследнике своего энумератора.

            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {   // Метод выводящий строковые отображения элементов массива в одну строку (сделан для демонстрации)
            return string.Join(", ", ToArray());
        }

        public List<T> ToList()
        {   // Метод возвращающий коллекцию ввиде коллекции List
            return baseArray.Take(baseArray.Length - (baseArray.Length - Count)).ToList();
        }

        public T[] ToArray()
        {   // Метод возвращающий коллекцию ввиде массива
            return baseArray.Take(baseArray.Length - (baseArray.Length - Count)).ToArray();
        }

        protected void ChangeArraySize(Func<int, int> func)
        {   // Внутренний метод изменяющий размер массива в соответствии с функцией передаваемой встроенным делегатом Func

            T[] tempArray = new T[func(baseArray.Length)];
            Array.Copy(baseArray, tempArray, Count);
            baseArray = tempArray;
        }


    }
}
