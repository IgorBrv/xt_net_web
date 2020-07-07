using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArrayLib
{
	class DynamicArrayEnumerator<T> : IEnumerator<T>, IEnumerable<T>
	{   // Самописный Енумератор для Динамического массива. Предоставляет возможности как обычного итерирования, так и зацикленного.
		// !!! Был заменен на yield

		private int curIndex = -1;
		private bool isLooped = false;
		private readonly AbstractDynamicArray<T> dynamicArray;
		public DynamicArrayEnumerator(AbstractDynamicArray<T> dynamicArray)
		{
			this.dynamicArray = dynamicArray;
		}

		public DynamicArrayEnumerator(AbstractDynamicArray<T> dynamicArray, bool isLooped)
		{	// Дополнительный конструктор для реализации возможности зацикленного итерирования.
			this.isLooped = isLooped;
			this.dynamicArray = dynamicArray;
		}

		public T Current { get; private set; }

		object IEnumerator.Current => Current;

		public void Dispose() { }	// На docs.microsoft в данной ситуации сказанно оставить пустую реализацию

		public bool MoveNext()
		{
			curIndex++;

			if (curIndex >= dynamicArray.Count)
			{
				Reset();
				if (!isLooped)
				{
					return false;
				}
				else
				{
					curIndex++;
				}
			}

			Current = dynamicArray[curIndex];
			return true;
		}

		public void Reset() => curIndex = -1;


		public IEnumerator<T> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}
