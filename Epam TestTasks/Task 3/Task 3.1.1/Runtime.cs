using System;
using Outputlib;

namespace Task_3_1_1
{
	// В кругу стоят N человек, пронумерованных от 1 до N. При ведении счета по кругу (каждый «раунд») вычёркивается каждый второй человек,
	//пока не останется один. Составить программу, моделирующую процесс.

	class Runtime
	{
		static readonly string name = " Задание \"Слабое звено\"";
		static readonly Draw draw = new Draw(name);		// Создаём объект универсального отрисовщика интерфейса окна консоли
		static void Main()
		{
			int step = 2;
			bool exit = false;
			int participants = 10;

			while (!exit)
			{
				
				string[] strings = {$"Введите желаемое число от 0 до 3",
									"К вводу допускаются только цифры от 0 до 3!",
									" 0. Выход\n 1. Начало симуляции\n 2. Смена колличества участников \n 3. Смена \"Шага\" выбывания\n"};
				int input = Int32.Parse(draw.Form(new int[] { 0, 0, 3 }, strings));		// Отрисуем главное меню и запросим ввод желаемого действия.
				switch (input)                                                          // Exceptions Parse не перехватываются, т.к. draw досконально проверяет корректность ввода
				{
					case 0:		// В случае ввода 0: Выход
						exit = true;
						break;
					case 1:     // В случае ввода 1: Запускаем симуляцию
						SelectionProcess sp = new SelectionProcess(participants, draw);
						sp.Run(step);
						participants = 10;		// После симуляции возвращаем дефолтное колличество участников и шаг
						step = 2;
						break;
					case 2:     // В случае ввода 2: Выводим окно смены колличества участников
						participants = ChangeNumOfParticipants(step);
						break;
					case 3:     // В случае ввода 3: Выводим окно смены шага выбывания
						step = ChangeStep(participants);
						break;
				}
			}
		}

		private static int ChangeNumOfParticipants(int step)
		{	// Вспомогательный метод отрисовывающий меню смены колличества участников
			string[] strings = {$"Введите желаемое колличество участников от {step} до 100",
								$"К вводу допускаются только цифры от {step} до 100!"};
			return Int32.Parse(draw.Form(new int[] { 0, step, 100 }, strings));    // Exceptions Parse не перехватываются, т.к. draw досконально проверяет корректность ввода
		}

		private static int ChangeStep(int participants)
		{   // Вспомогательный метод отрисовывающий меню смены шага выбывания
			string[] strings = {$"Введите желаемый шаг удаления участников от 1 до {participants}",
								$"К вводу допускаются только цифры от 1 до {participants}!"};

			return Int32.Parse(draw.Form(new int[] { 0, 1, participants }, strings));     // Exceptions Parse не перехватываются, т.к. draw досконально проверяет корректность ввода
		}
	}
}
