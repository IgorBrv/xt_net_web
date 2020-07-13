using System.Collections.Generic;


namespace DynamicArrayLib
{
    public class CycledDynamicArray<T> : AbstractDynamicArray<T>
    {   // Класс CycledDynamicArray:  циклический динамический массив (CycledDynamicArray) на основе DynamicArray, отличающийся тем,
        // что при использовании foreach после последнего элемента должен снова идти первый и так по кругу.

        public CycledDynamicArray() : base() { }

        public CycledDynamicArray(int capacity) : base(capacity) { }

        public CycledDynamicArray(IEnumerable<T> collection) : base(collection) { }

        public override IEnumerator<T> GetEnumerator()
        {
            int index = -1;

            while (true)
            {
                index++;

                if (index == Count)
                {
                    index = 0;
                }

                yield return baseArray[index];
            }
        }

    }
}
