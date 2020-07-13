using Outputlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaTime
{
	class Simulation
	{   // Класс приглашающий покупателей в пиццерию и занимающийся отрисовкой происходящего в окно консоли

		private readonly Pizzeria pizzeria = new Pizzeria();
		private readonly Mutex asyncDelay = new Mutex();
		private List<Customer> servedCustomers;
		private List<Customer> custList;
		private bool keyCtrlExit = false;
		private bool exit = false;
		private int custNum = 0;

		public void Run(int customersAmount)
		{
			Console.CursorVisible = false;

			LaunchKeysControl();						// Запуск обработки ввода с клавиатуры
			custList = new List<Customer>();			// Список клиентов в очереди
			servedCustomers = new List<Customer>();		// Список клиентов получивших свой заказ
			StringBuilder sb = new StringBuilder();
			string[] strings = { "Запущен процесс симуляции. Клиенты в очереди: ", "", "" };

			while (!exit)
			{   // Цикл симуляции

				Console.Clear();
				Output.Print("b", "c", " Симуляция работы пиццерии. Для выхода нажмите ESC".PadRight(120));
				Console.WriteLine($" Пиццы в процессе приготовления: {pizzeria.GetPizzasList()}\n");

				while (custList.Count < customersAmount)
				{	// Цикл следящий за колличеством человек в очереди
					custNum++;
					custList.Add(new Customer(custNum, CustomerServed, CustomerGoingOut, pizzeria));
				}

				// Подшивание статуса всех человек в заведении в строку, для отображения на экране 
				// Linq Ради Linq, для "набивания руки")
				sb.Append(string.Join("", custList.Select(value => value.State)));
				sb.Append("\n Обслуженные клиенты:\n\n");
				sb.Append(string.Join("", servedCustomers.Select(value => value.State)));

				Console.WriteLine(sb.ToString());
				Thread.Sleep(TimeSpan.FromSeconds(1));
				sb.Clear();
			}

			Console.CursorVisible = true;
			Console.Clear();
		}

		private void CustomerServed(Customer cust)
		{	// Метод, перемещающий клиента из списка ожидания в зал для поглощения пиццы.
			asyncDelay.WaitOne();   // Синхронизация для поочерёдного доступа потоков к спискам клиентов
			{
				custList.Remove(cust);
				cust.GoingOut();
				servedCustomers.Add(cust);
			}
			asyncDelay.ReleaseMutex();
		}

		private void CustomerGoingOut(Customer cust)
		{	// Метод, удаляющий откушавшего клиента из списка клиентов
			asyncDelay.WaitOne();	// Синхронизация для поочерёдного доступа потоков к спискам клиентов
			{
				servedCustomers.Remove(cust);
			}
			asyncDelay.ReleaseMutex();
		}

		private async void LaunchKeysControl()
		{	// Асинхронный метод запускающий процесс захвата ввода с клавиатуры (для выхода)
			await Task.Run(() => KeysControl());
		}

		private void KeysControl()
		{	// Метод захвата ввода с клавиатуры

			ConsoleKeyInfo key;

			while (!keyCtrlExit)
			{
				key = Console.ReadKey();

				if (key.Key == ConsoleKey.Escape)
				{
					keyCtrlExit = true;
					pizzeria.ClosingTIme();
					exit = !exit;
				}
				Thread.Sleep(100);
			}
		}
	}
}
