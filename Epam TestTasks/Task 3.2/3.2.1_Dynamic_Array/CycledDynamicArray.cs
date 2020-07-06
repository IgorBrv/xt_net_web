using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArrayLib
{
    public class CycledDynamicArray<T> : AbstractDynamicArray<T>
    {   // Класс CycledDynamicArray:  циклический динамический массив (CycledDynamicArray) на основе DynamicArray, отличающийся тем,
        // что при использовании foreach после последнего элемента должен снова идти первый и так по кругу.

        private DynamicArrayEnumerator<T> Enumerator;

        public CycledDynamicArray() : base() { }

        public CycledDynamicArray(int capacity) : base(capacity) { }

        public CycledDynamicArray(IEnumerable<T> collection) : base(collection) { }

        public override IEnumerator<T> GetEnumerator()
        {
            if (Enumerator == null)
            {
                Enumerator = new DynamicArrayEnumerator<T>(this, true);
            }

            return Enumerator.GetEnumerator();
        }

    }
}
