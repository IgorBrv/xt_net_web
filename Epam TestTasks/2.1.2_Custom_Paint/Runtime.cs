using System;
using CStringLib;  // Использование собственной библиотеки по заданию 2.1.1**
using Outputlib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Custom_Paint
{
	class Runtime
	{	// Класс рабочего процесса программы

		private readonly User user;

		public Runtime(User user)
		{
			this.user = user;
		}

		public bool MainMenu()
		// Метод определяющий содержание и поведение главного меню программы
		{
			bool exit = false;

			while (!exit)
			{
				string[] strings = {$"{user.Name}, выберите желаемое действие",
									$"{user.Name}, к вводу допускаются только числа 0, 1, 2, 3, 4!!!",
									"  1. Добавить фигуру\n  2. Вывести фигуры на экран\n  3. Очистить холст\n  4. Сменить пользователя\n  0. Выход\n" };
				string input = Draw.Form(new int[] { 0, 0, 4}, strings);

				switch (Int32.Parse(input))
				{
					case 1:
						NewShapeMenu(); break;
					case 2:
						ShowShapes(); break;
					case 3:
						ClearSelection(); break;
					case 4:
						exit = true; break;
					case 0:
						return false;
					default:
						break;
				}
			}
			return true;
		}

		private void NewShapeMenu()
		// Метод, определяющий содержание и поведение подменю добавления фигуры
		{
			string[] strings = { $"{ user.Name }, выберите фигуру, которую желаете создать:",
								 $"{ user.Name }, к вводу допускаются только числа 0, 1, 2, 3, 4, 5!!!",
								  "  1. Круг\n  2. Кольцо\n  3. Квадрат\n  4. Прямоугольник\n  5. Паралеллограмм\n  0. Отмена\n" };
			string input = Draw.Form(new int[] { 0, 0, 5 }, strings);

			switch (Int32.Parse(input))
			{
				case 1:  // Создание круга
					
					strings = new string[] { $"{ user.Name }, введите координаты центра и радиус круга через запятую:",
											 $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"x, y, радиус\"!"};
					string[] data = Draw.Form(new int[] { 1, 3, 1, 100 }, strings).Split(',');

					try
					{
						Circle shape = new Circle(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true );
					}
					
					break;

				case 2:  // Создание кольца

					strings = new string[] { $"{ user.Name }, введите координаты центра, радиус наружной и внутренней окружностей через запятую:",
											 $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"x, y, радиус, внутренний радиус\"!"};
					data = Draw.Form(new int[] { 1, 4, 1, 100 }, strings).Split(',');

					try
					{
						Ring shape = new Ring(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), Int32.Parse(data[3]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 3:  // Создание квадрата

					strings = new string[] { $"{ user.Name }, введите координаты левого верхнего угла и длину стороны квадрата через запятую:",
											 $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"x, y, длина стороны\"!"};
					data = Draw.Form(new int[] { 1, 3, 1, 100 }, strings).Split(',');

					try
					{
						Square shape = new Square(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 4:  // Создание прямоугольника

					strings = new string[] { $"{ user.Name }, введите координаты левого верхнего угла и длину сторон прямоугольника через запятую:",
											 $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"x, y, сторона_а, сторона_б\"!"};
					data = Draw.Form(new int[] { 1, 4, 1, 100 }, strings).Split(',');

					try
					{
						Rectangle shape = new Rectangle(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), Int32.Parse(data[3]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 5:  // Создание паралеллограмма

					strings = new string[] { $"{ user.Name }, введите координаты левого верхнего угла, длину сторон паралеллограмма, угол наклона через запятую:",
											 $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"x, y, радиус, внутренний радиус, угол\"!"};
					data = Draw.Form(new int[] { 1, 5, 1, 100 }, strings).Split(',');

					try
					{
						Parallelogram shape = new Parallelogram(Int32.Parse(data[0]), Int32.Parse(data[1]), Int32.Parse(data[2]), Int32.Parse(data[3]), Int32.Parse(data[4]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 0: break;  // Выход
			}
		}

		private Colors ColorsMenu()
		// Метод, определяющий содержание и поведение подменю выбора цвета
		{
			string[] strings = { $"{ user.Name }, выберите цвет для фигуры:",
								 $"{ user.Name }, к вводу допускаются только числа 1, 2, 3!!!",
								  "  1. Белый\n  2. Красный\n  3. Оранжевый\n  4. Жёлтый\n  5. Зелёный\n  6. Синий\n  7. Фиолетовый\n  8. Чёрный" };
			string input = Draw.Form(new int[] { 0, 1, 8 }, strings);

			switch (Int32.Parse(input))
			{
				case 1:
					return Colors.White;
				case 2:
					return Colors.Red;
				case 3:
					return Colors.Orange;
				case 4:
					return Colors.Yellow;
				case 5:
					return Colors.Green;
				case 6:
					return Colors.Blue;
				case 7:
					return Colors.Violet;
				case 8:
					return Colors.Black;
				default:
					return Colors.Default;
			}
		}

		private void ShowShapes()
		// Метод, определяющий содержание и поведение подменю отображения списка нарисованных фигур
		{
			if (user.Shapes.Count == 0)
			{
				Draw.Form(new int[] { 3 }, new string[] { "", $"Список фигур пуст!" }, true);
			}
			else
			{
				List<string> strings = new List<string> { " Нарисованные фигуры: ", "" };
				StringBuilder sb = new StringBuilder();
				int count = 1;
				foreach (Shapes shape in user.Shapes)
				{
					sb.Append($" {count}. {shape.About}\n");
					count++;
				}
				strings.Add(sb.ToString());
				Draw.Form(new int[] { 3 }, strings.ToArray());
			}
		}

		private void ClearSelection()
		// Метод, определяющий содержание и поведение подменю очистки списка фигур
		{
			if (user.Shapes.Count == 0)
			{
				Draw.Form(new int[] { 3 }, new string[] { "", $"Список фигур пуст!" }, true);
			}
			else
			{
				user.Shapes.Clear();
				Draw.Form(new int[] { 3 }, new string[] { "Список нарисованых фигур очищен!", ""});
			}
		}
	}
}
