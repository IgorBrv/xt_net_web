using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicArrayLib;

namespace DynamicArrayDemonstration
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Демонстрация работы бибоиотеки Dynamic_Array");
			DynamicArray<int> da = new DynamicArray<int>();
			Console.WriteLine($"Создание экземпляра DynamicArray конструктором без параметров, вместимость: {da.Capacity}");
			DynamicArray<int> da2 = new DynamicArray<int>(4);
			Console.WriteLine($"Создание экземпляра DynamicArray конструктором с параметром 4, вместимость: {da2.Capacity}");
			List<int> lst1 = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			DynamicArray<int> da3 = new DynamicArray<int>(lst1);
			Console.WriteLine($"Создание экземпляра DynamicArray конструктором принимающим на вход другую коллекцию IEnumerable\n" +
				$"Длина коллекции List: {lst1.Count}, вместимость: {lst1.Capacity}, содержимое: {string.Join(", ", lst1)};\n" +
				$"Длина коллекции DynamicArray: {da3.Count}, вместимость: {da3.Capacity}, содержимое: {da3}");
			Console.WriteLine("Метод Add, позволяющий добавлять в конец массива новые данные. При нехватке места для добавления элемента массив удваивается: ");
			int count = 1;
			while (count < 6)
			{
				da2.Add(count);
				Console.WriteLine($"Колличество элементов: {da2.Count}, вместимость: {da2.Capacity}");
				count++;
			}
			Console.WriteLine($"Метод AddRange добавляющий в конец массива содержимое коллекции IEnumerable. Содержимое коллекции da2 до добавления lst1: {da2}, вместимость: {da2.Capacity}");
			da2.AddRange(lst1);
			Console.WriteLine($"Метод AddRange добавляющий в конец массива содержимое коллекции IEnumerable. Содержимое коллекции da2 после добавления lst1: {da2}, вместимость: {da2.Capacity}");
			da2.Remove(5);
			Console.WriteLine($"Метод Remove удаляющий из коллекции пепрвое вхождение заданного элемента (5). Содержимое коллекции da2 после удаления 5: {da2}, вместимость: {da2.Capacity}");
			da2.Insert(4, 5);
			da2.Insert(4, 5);
			Console.WriteLine($"Метод Insert, позволяющую вставить объект по нужному индексу Содержимое коллекции da2 после двойного добавления 5: {da2}, вместимость: {da2.Capacity}");
			Console.WriteLine($"Свйойство Lenght - получение колличества элементов (В данном случае Count): {da2.Count}, вместимость: {da2.Capacity}");
			Console.WriteLine("Реализация IEnumerable и Ienumerable<T>");
			foreach (int i in da3)
			{
				Console.WriteLine(i);
			}
			Console.WriteLine("Реализация доступа по индексу");
			for (int i = 0; i < da3.Count; i++)
			{
				Console.WriteLine(da3[i]);
			}
			
			try
			{
				Console.WriteLine(da3[10]);
			}
			catch(ArgumentOutOfRangeException ex)
			{
				Console.WriteLine($"Демонстрация ошибки при доступе мимо индекса: {ex.Message}");
			}

			Console.WriteLine("Доступ к элементам с конца, при использовании отрицательного индекса:");

			count = -1;
			while (count >= da3.Count*-1)
			{
				Console.WriteLine($"Индекс: {count}; содержимое: {da3[count]}");
				count--;
			}

			Console.WriteLine($"Метод ToArray возввращающий обычный массив: {string.Join(", ", da3.ToArray())}");

			DynamicArray<int> da4 = da3.Clone() as DynamicArray<int>;

			Console.WriteLine($"Реализация интерфейса IClonable: {string.Join(", ", da4)}");

			Console.WriteLine($"Перегрузка оператора Equals: {da3.Equals(da4)}, ==, {da3 == da4}");

			CycledDynamicArray<int> cycledArray = new CycledDynamicArray<int>(da3);

			Console.WriteLine("Массив CycledDynamicArray на базе DynamicArray с зацикленным итератором:");
			count = 0;

			foreach (int i in cycledArray)
			{
				Console.WriteLine(i);
				count++;
				if (count > 30) break;
			}

			Console.ReadLine();
		}
	}
}
