using System;
using CStringLib;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Custom_String
{   // Напишите собственный класс, описывающий строку как массив символов. Реализуйте для этого класса типовые операции (сравнение, конкатенация, поиск символов, конвертация из/в массив символов).
	// Подумайте, какие функции вы бы добавили к имеющемуся в .NET функционалу строк (достаточно 1-2 функций).
	// Вариант со * - подумайте над использованием в своем классе функционала индексатора(indexer). Реализуйте его для своей строки.
	// Вариант с ** - попробуйте создать из своей сборки переносимую библиотеку(DLL). Осмысленно назовите её, а также namespace и сам класс.Попробуйте использовать написанный вами класс в другом проекте.
	//
	// Реализованые операции:
	// Индексатор (Доступ к символам по индексу), Энумератор, .Lenght, .ToArray, .ToList, .ToString, .Concat, +, ++, *, Insert, Equals, GetHashCode, ==, !=, Find, RFind, FindNext, RFindNext.
	//
	// Функции, которые отсутствуют в имеющемся .NET функционале строк:  ++, *, RFind, FindNext, RFindNext
	//
	// Собственный класс строки заключён в переносную библиотеку DLL CStringLib. Данная программа использует данную библиотеку для демонстрации функцонала.
	class Program
	{
		static void Main(string[] args)
		{
			CString customstring = new CString("qwerty");

			// Строковое отображение:
			Console.WriteLine($"Строковое отображение: {customstring}");

			// Отображение длины:
			Console.WriteLine($"Отображение длины: {customstring.Length}");

			// Конвертация в массив Array:
			Console.WriteLine($"Конвертация в массив: [{string.Join(", ", customstring.ToArray())}]");

			// Конкатенация со строкой:
			Console.WriteLine($"Конкатенация со строкой через .concat: {customstring.Concat("two")}");

			// Строковое отображение:
			Console.WriteLine($"Строковое отображение: {customstring}");

			// Конкатенация со строкой:
			Console.WriteLine($"Конкатенация со строкой через +: {customstring + "two"}");

			// Строковое отображение:
			Console.WriteLine($"Строковое отображение: {customstring}");

			// Умножение строки:
			Console.WriteLine($"Умножение строки на 3: {customstring * 3}");

			// Присвоение объекту CString объекта типа string:
			customstring = "qwerty";

			// Intsert:
			customstring.Insert(0, "qwerty");
			Console.WriteLine($"Insert в строку: {customstring}");

			//Поиск по строке
			List<int> indexes = new List<int>();
			int index = 0;
			while (index >= 0)
			{
				index = customstring.FindNext("qw");
				if (index >= 0)
				{
					indexes.Add(index);
				}
			}

			//Использование индексатора
			Console.WriteLine($"Отображение индексов из строки {customstring}:");
			foreach (int i in indexes)
			{
				Console.WriteLine($"Индекс {i}->{i}+1: {customstring[i]}{customstring[i + 1]}");
			}

			indexes.Clear();

			//Поиск по строке справа
			index = 0;
			while (index >= 0)
			{
				index = customstring.RFindNext("qw");
				if (index >= 0)
				{
					indexes.Add(index);
				}
			}

			//Использование индексатора
			Console.WriteLine($"Отображение индексов из строки {customstring}:");
			foreach (int i in indexes)
			{
				Console.WriteLine($"Индекс {i}->{i}+1: {customstring[i]}{customstring[i + 1]}");
			}
			Console.Read();
		}
	}



}
