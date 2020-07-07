using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicArrayLib
{
	public static class IEnumerableExtension
	{
		public static DynamicArray<T> ToDynamicArray<T>(this IEnumerable<T> Collection)
		{	// Метод расширения, добавляющий метод ToDynamicArray элементам реализующим интерфейс IEnumerable

			return new DynamicArray<T>(Collection);
		}
	}
}
