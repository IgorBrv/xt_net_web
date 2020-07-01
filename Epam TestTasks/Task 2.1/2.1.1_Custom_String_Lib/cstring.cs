using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;


namespace CStringLib
{
	// Напишите собственный класс, описывающий строку как массив символов. Реализуйте для этого класса типовые операции (сравнение, конкатенация, поиск символов, конвертация из/в массив символов).
	// Подумайте, какие функции вы бы добавили к имеющемуся в .NET функционалу строк (достаточно 1-2 функций).
	// Вариант со * - подумайте над использованием в своем классе функционала индексатора(indexer). Реализуйте его для своей строки.
	// Вариант с ** - попробуйте создать из своей сборки переносимую библиотеку(DLL). Осмысленно назовите её, а также namespace и сам класс.Попробуйте использовать написанный вами класс в другом проекте.
	//
	// Реализованые операции:
	// Индексатор (Доступ к символам по индексу), Энумератор, .Lenght, .ToArray, .ToList, .ToString, .Concat, +, ++, *, Insert, Equals, GetHashCode, ==, !=, Find, RFind, FindNext, RFindNext.
	//
	// Функции, которые отсутствуют в имеющемся .NET функционале строк:  ++, *, RFind, FindNext, RFindNext

	public class CString : IEnumerable
	{   // Собственный класс строки, который описывает строку как массив символов

		private char[] cstr;

		// Вспомогательный словарь для методов поиска:
		private Dictionary<string, int> findIndex;

		public CString(string str)
		{   // Конструктор объекта структуры:
			cstr = str.ToArray();
			findIndex = new Dictionary<string, int>();
		}


		public char this[int index]
		{   // Индексатор класса
			get
			{
				return cstr[index];
			}
			set
			{
				cstr[index] = value;
			}
		}

		public IEnumerator GetEnumerator()
		{   // Реализация интерфейса  IEnumerable
			return cstr.GetEnumerator();
		}


		public int Length
		{	// Возвращает длину массива cstr;
			get
			{
				return cstr.Length;
			}
		}


		// ОПРЕДЕЛЕНИЯ И ПЕРЕОПРЕДЕЛЕНИЯ МЕТОДОВ И ОПЕРАЦИЙ ПРЕОБРАЗОВАНИЯ ТИПОВ


		public char[] ToArray()
		{   // Преобразует объект CString в массив []Char
			return cstr;
		}

		public List<char> ToList()
		{   // Преобразует объект CString в список List<char
			return cstr.ToList();
		}

		public override string ToString()
		{   // Не особо нужный метод при наличии перегруженой операции преобразования типов в string, но, пускай будет
			return string.Join("", cstr);
		}

		public static implicit operator CString(string str)
		{   // Определяет действие при присваивании объекту CString строкового значения
			return new CString(str);
		}

		public static implicit operator CString(int num)
		{   // Определяет действие при присваивании объекту CString значения типа int
			return new CString(num.ToString());
		}

		public static implicit operator string(CString cstring)
		{   // Определяет действие при явном приведении объекта CString к обекту типа string
			return string.Join("", cstring.cstr);
		}

		public static explicit operator char[](CString cstring)
		{   // Определяет действие при явном приведении объекта CString к обекту типа char[]
			return cstring.cstr;
		}


		// ПЕРЕОПРЕДЕЛЕНИЕ ОПЕРАТОРОВ И МЕТОДОВ КОНКАТЕНАЦИИ, УМНОЖЕНИЯ И СЛОЖЕНИЯ


		public CString Concat(string str)
		{   // Определяет метод конкатенирования со строкой
			return new CString(string.Join("", cstr) + str);
		}

		public CString Concat(CString cstring)
		{   // Определяет метод конкатенирования с объектом типа CString
			return new CString(string.Join("", cstr.Concat(cstring.cstr)));
		}

		public static CString operator +(CString item, int value)
		{   // Определяет метод конкатенирования с объектом типа int
			return item.Concat(value.ToString());
		}

		public static CString operator +(CString item, string value)
		{   // Определяет метод конкатенирования с объектом типа string
			return item.Concat(value);
		}

		public static CString operator +(CString item, CString item2)
		{   // Определяет метод конкатенирования с объектом типа CString
			return item.Concat(item2);
		}

		public static CString operator ++(CString item)
		{   // Определяет метод удваивающий содержимое CString
			return item * 2;
		}

		public static CString operator *(CString item, int multiplier)
		{   // Перегруженый оператор перемножения, создаёт новый массив с заданным числом повтороений изначальной строки
			char[] temp = new char[item.Length * multiplier];
			int count = 0;

			while (multiplier > 0)
			{
				foreach (char i in item.ToArray())
				{
					temp[count] = i;
					count++;
				}
				multiplier -= 1;
			}

			item.cstr = temp;
			return item;
		}

		public void Insert(int index, string str)
		{   // Метод Insert, который вставляет строку в имеющуюся не создавая нового экземпляра класса 
			// (Цикл можно заменить на встроенные методы concat/take/skip, но с циклом по моему мнению происходит меньше операций
			char[] temp = new char[cstr.Length + str.Length];
			int count = 0;

			foreach (char i in cstr)
			{
				if (count == index)
				{
					foreach (char x in str)
					{
						temp[count] = x;
						count++;
					}
				}
				temp[count] = i;
				count++;
			}

			cstr = temp;
		}


		// ПЕРЕОПРЕДЕЛЕНИЕ МЕТОДОВ И ОПЕРАТОРОВ СРАВНЕНИЯ:


		public static bool operator ==(CString item, string value)
		{   // Сравнение со строкой при помощи оператора ==
			bool equals = false;
			if (item.ToString().Equals(value)) equals = true;
			return equals;
		}

		public static bool operator !=(CString item, string value)
		{   // Сравнение со строкой при помощи оператора !=
			bool equals = true;
			if (item.ToString().Equals(value)) equals = false;
			return equals;
		}

		public static bool operator ==(CString item, CString item2)
		{   // Сравнение с другим объектом CString при помощи оператора ==
			bool equals = false;
			if (item.Equals(item2)) equals = true;
			return equals;
		}

		public static bool operator !=(CString item, CString item2)
		{   // Сравнение с другим объектом CString при помощи оператора !=
			bool equals = true;
			if (item.Equals(item2)) equals = false;
			return equals;
		}

		public override int GetHashCode()
		{   // Переопределение метода GetHashCode()
			return cstr.GetHashCode();
		}

		public override bool Equals(Object obj)
		{   // Переопределение метода Equals()
			if (this.GetHashCode() == obj.GetHashCode()) return true;
			return false;
		}


		// РЕАЛИЗАЦИЯ МЕТОДОВ ПОИСКА, ПОИСКА СЛЕДУЮЩЕГО ЭЛЕМЕНТА, ПОИСКА СПРАВА


		public int FindNext(string i)
		{   // Метод поиска следующего элемента
			if (findIndex.ContainsKey(i))
			{
				return Find(i, findIndex[i] + 1);
			}
			else
			{
				return Find(i);
			}
		}

		public int RFindNext(string i)
		{   // Метод поиска следующего элемента справа
			if (findIndex.ContainsKey(i))
			{
				return RFind(i, findIndex[i] - 1);
			}
			else
			{
				return RFind(i);
			}
		}


		public int Find(string i, int index = 0)
		{   // Самописный метод поиска, позволяет реализовывать поиск подстроки, поиск следующей подстроки
			int count = 0;
			int secondCount = 1;
			bool error = false;

			for (int x = index; x < cstr.Length; x++)
			{
				if (cstr[x] == i[count])
				{
					count++;
					for (int z = x + 1; z < x + i.Length; z++)
					{
						if (cstr[z] != i[secondCount])
						{
							error = true;
						}
						secondCount++;
					}
					if (!error)
					{
						if (findIndex.ContainsKey(i))
						{
							findIndex[i] = x;
						}
						else findIndex.Add(i, x);
						return x;
					}
					count = 0;
					secondCount = 1;
					error = false;
				}
			}
			if (findIndex.ContainsKey(i))
			{
				findIndex[i] = -1;
			}
			else findIndex.Add(i, -1);
			return -1;
		}


		public int RFind(string i, int index = -2)
		{   // Самописный метод поиска справа, позволяет реализовывать поиск подстроки, поиск следующей подстроки
			int count = 0;
			int secondCount = 1;
			bool error = false;

			if (index < -1)
			{
				index = cstr.Length;
			}
			else if (index == -1)
			{
				if (findIndex.ContainsKey(i))
				{
					findIndex[i] = -1;
				}
				else findIndex.Add(i, -1);
				return -1;
			}


			for (int x = index - 1; x >= 0; x--)
			{
				if (cstr[x] == i[count])
				{
					count++;
					for (int z = x + 1; z < x + i.Length; z++)
					{
						if (cstr[z] != i[secondCount])
						{
							error = true;
						}
						secondCount++;
					}
					if (!error)
					{
						if (findIndex.ContainsKey(i))
						{
							findIndex[i] = x;
						}
						else findIndex.Add(i, x);
						return x;
					}
					count = 0;
					secondCount = 1;
					error = false;
				}
			}
			if (findIndex.ContainsKey(i))
			{
				findIndex[i] = -1;
			}
			else findIndex.Add(i, -1);
			return -1;
		}

	}
}
