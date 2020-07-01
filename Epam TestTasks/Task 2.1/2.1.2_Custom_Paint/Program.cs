using ValidatorLib;

namespace Custom_Paint
{
	class Program
	{
		static void Main()
		{
			bool cont = true;
			while (cont)
			{   // Инициализация программы, создания объекта класса пользователь и передача экземпляра в создаваемый объект класса runtime.
				// Для отрисовки всего используется единый класс Draw, в который передаются переменные задающие желаемый режим (Ввод чисел, массива чисел, строк), и строки.
				// Процесс инициализации закольцован в цикл while для возможности смены пользователя.

				string[] strings = {"Введите имя пользователя: ",
									"К вводу допускаются только буквы! Имя может состоять из трёх слов!"};

				string username = Validator.Fix(Draw.Form(new int[] { 2, 3 }, strings), ' ');
				User user = new User(username);
				Runtime runtime = new Runtime(user);
				cont = runtime.MainMenu();
			}
		}
	}
}
