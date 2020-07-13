using System;
using Outputlib;

namespace PizzaTime
{   // Смоделируйте работу пиццерии в вашем приложении.
	// Две ключевые сущности: пользователь и пиццерия взаимодействуют через заказ и пиццу.
	// Пользователь делает заказ в заведении, после чего ждет оповещения о том, что пицца готова. 
	// Например – его имя высветится на табло. После этого пользователь сам забирает пиццу.

	class Program
	{
		static readonly string name = " Симуляция работы пиццерии";
		static readonly Draw draw = new Draw(name);

		static void Main()
		{
			bool exit = false;
			int customersNumber = 4;
			string[] strings = { "Выберите желаемое действие:", "К вводу допускаются только числа от 1 до 2!", $" 0. Выход\n 1. Начало симуляции\n 2. Смена колличества покупателей (В данный момент: {customersNumber})\n" };

			while (!exit)
			{
				int input = Int32.Parse(draw.Form(new int[] { 0, 0, 2 }, strings)); // Parse используется т.к. класс Draw проводит избыточное колличество проверок ввода

				switch (input)
				{
					case 0:
						exit = true;
						break;
					case 1:
						Simulation simulation = new Simulation();
						simulation.Run(customersNumber);
						break;
					case 2:
						customersNumber = ChangeCustomersNumber();
						strings[2] = $" 0. Выход\n 1. Начало симуляции\n 2. Смена колличества покупателей (В данный момент: {customersNumber})\n";
						break;
				}
			}
		}

		private static int ChangeCustomersNumber()
		{	// Меню выбора колличества покупателей
			string[] strings = { "Введите желаемое колличество покупателей (до 15):", "К вводу допускаются только числа от 1 до 15!" };
			return Int32.Parse(draw.Form(new int[] { 0, 1, 15 }, strings));
		}
	}
}
