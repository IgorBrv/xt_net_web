using System;
using System.Collections.Generic;
using System.Text;


namespace Custom_Paint
{
	class Runtime
	{	// Класс рабочего процесса программы, определяет состав и действия меню программы.
		// Содержание новой страницы передаётся в класс draw для отрисовки и запроса ввода.

		private readonly User user;
		private char[] toSplit = new char[] { ',', ' ' };

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
					
					string[] stringsP1 = new string[] { $"{ user.Name }, введите координаты круга (о координатах см. в комментариях) через запятую:",
														$"{ user.Name }, к вводу допускаются только числа от -100 до 100 в формате \"x, y\"!"};
					string[] stringsP2 = new string[] { $"{ user.Name }, введите диаметр круга:",
														$"{ user.Name }, к вводу допускаются только числа от 1 до 100!"};
					string[] dataP1 = Draw.Form(new int[] { 1, 2, -100, 100 }, stringsP1).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					string   dataP2 = Draw.Form(new int[] { 0, 1, 100 }, stringsP2);

					try
					{	// Используется parse, т.к. значения уже прогонялись через TryParce в классе Input
						Circle shape = new Circle(Int32.Parse(dataP1[0]), Int32.Parse(dataP1[1]), Int32.Parse(dataP2), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true );
					}
					
					break;

				case 2:  // Создание кольца

					stringsP1 = new string[] { $"{ user.Name }, введите координаты кольца (о координатах см. в комментариях) через запятую:",
											   $"{ user.Name }, к вводу допускаются только числа от -100 до 100 в формате \"x, y\"!"};
					stringsP2 = new string[] { $"{ user.Name }, введите внешний диаметр кольца, внутренний диаметр кольца:",
											   $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"d1, d2\" (d1 > d2)!"};
							 dataP1 = Draw.Form(new int[] { 1, 2, -100, 100 }, stringsP1).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					string[] dataP3 = Draw.Form(new int[] { 1, 2,  1, 100 }, stringsP2).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);

					try
					{
						Ring shape = new Ring(Int32.Parse(dataP1[0]), Int32.Parse(dataP1[1]), Int32.Parse(dataP3[0]), Int32.Parse(dataP3[1]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 3:  // Создание квадрата

					stringsP1 = new string[] { $"{ user.Name }, введите координаты квадрата через запятую или пробел:",
											   $"{ user.Name }, к вводу допускаются только числа от -100 до 100 в формате \"x, y\"!"};
					stringsP2 = new string[] { $"{ user.Name }, введите длину стороны квадрата:",
											   $"{ user.Name }, к вводу допускаются только числа от 1 до 100!"};
					dataP1 = Draw.Form(new int[] { 1, 2, -100, 100 }, stringsP1).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					dataP2 = Draw.Form(new int[] { 0, 1, 100 }, stringsP2);

					try
					{
						Square shape = new Square(Int32.Parse(dataP1[0]), Int32.Parse(dataP1[1]), Int32.Parse(dataP2), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 4:  // Создание прямоугольника

					stringsP1 = new string[] { $"{ user.Name }, введите координаты прямоугольника через запятую или пробел:",
											   $"{ user.Name }, к вводу допускаются только числа от -100 до 100 в формате \"x, y\"!"};
					stringsP2 = new string[] { $"{ user.Name }, введите длины сторон прямоугольника через запятую:",
											   $"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"a, b\"!"};
					dataP1 = Draw.Form(new int[] { 1, 2, -100, 100 }, stringsP1).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					dataP3 = Draw.Form(new int[] { 1, 2, 1, 100 }, stringsP2).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);

					try
					{
						Rectangle shape = new Rectangle(Int32.Parse(dataP1[0]), Int32.Parse(dataP1[1]), Int32.Parse(dataP3[0]), Int32.Parse(dataP3[1]), ColorsMenu());
						user.Shapes.Add(shape);
						Draw.Form(new int[] { 3 }, new string[] { $"Фигура успешно создана!" });
					}
					catch (Exception e)
					{
						Draw.Form(new int[] { 3 }, new string[] { "", $"{e.Message}" }, true);
					}

					break;

				case 5:  // Создание паралеллограмма

							 stringsP1 = new string[] { $"{ user.Name }, введите координаты паралеллограмма через запятую или пробел:",
														$"{ user.Name }, к вводу допускаются только числа от -100 до 100 в формате \"x, y\"!"};
							 stringsP2 = new string[] { $"{ user.Name }, введите длины сторон паралеллограмма через запятую:",
														$"{ user.Name }, к вводу допускаются только числа от 1 до 100 в формате \"a, b\"!"};
					string[] stringsP3 = new string[] { $"{ user.Name }, введите угол наклона паралеллограмма через запятую:",
														$"{ user.Name }, к вводу допускаются только числа от -360 до 360!"};
					dataP1 = Draw.Form(new int[] { 1, 2, -100, 100 }, stringsP1).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					dataP3 = Draw.Form(new int[] { 1, 2, 1, 100 }, stringsP2).Split(toSplit, StringSplitOptions.RemoveEmptyEntries);
					dataP2 = Draw.Form(new int[] { 0, 0, 360 }, stringsP3);

					try
					{
						Parallelogram shape = new Parallelogram(Int32.Parse(dataP1[0]), Int32.Parse(dataP1[1]), Int32.Parse(dataP3[0]), Int32.Parse(dataP3[1]), Int32.Parse(dataP2), ColorsMenu());
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
								  "[cl][bw]  1. Белый\n[cl][wb]  2. Чёрный\n[cl][br]  3. Красный\n[cl][bo]  4. Оранжевый\n[cl][by]  5. Жёлтый\n[cl][bg]  6. Зелёный\n[cl][bs]  7. Синий\n[cl][bv]  8. Фиолетовый" };
			string input = Draw.Form(new int[] { 0, 1, 8 }, strings);

			switch (Int32.Parse(input))
			{
				case 1:
					return Colors.White;
				case 2:
					return Colors.Black;
				case 3:
					return Colors.Red;
				case 4:
					return Colors.Orange;
				case 5:
					return Colors.Yellow;
				case 6:
					return Colors.Green;
				case 7:
					return Colors.Blue;
				case 8:
					return Colors.Violet;
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
					string colorCode = GenerateColourString(shape.Color);
					sb.Append($"{colorCode} {count}. {shape.About}\n");
					count++;
				}
				strings.Add(sb.ToString());
				Draw.Form(new int[]  { 3 }, strings.ToArray());
			}
		}

		private string GenerateColourString(Colors Color)
		// Вспомогательный метод формирующий строковый код цвета для класса Draw
		{
			switch (Color)
			{
				case Colors.White:
					return "[cl][bw]";
				case Colors.Black:
					return "[cl][wb]";
				case Colors.Red:
					return "[cl][br]";
				case Colors.Orange:
					return "[cl][bo]";
				case Colors.Yellow:
					return "[cl][by]";
				case Colors.Green:
					return "[cl][bg]";
				case Colors.Blue:
					return "[cl][bs]";
				case Colors.Violet:
					return "[cl][bv]";
				default:
					return string.Empty;
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
				Draw.Form(new int[] { 3 }, new string[] { "Список нарисованых фигур очищен!" });
			}
		}
	}
}
