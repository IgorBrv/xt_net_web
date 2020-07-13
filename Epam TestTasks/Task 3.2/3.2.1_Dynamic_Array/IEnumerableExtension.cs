using System.Collections.Generic;

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
