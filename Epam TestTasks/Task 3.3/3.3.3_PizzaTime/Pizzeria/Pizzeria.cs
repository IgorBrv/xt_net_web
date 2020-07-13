using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaTime
{
	class Pizzeria
	{	// Класс объекта пиццерии

		private readonly Dictionary<int, AbstractPizza> PizzasInProcecss = new Dictionary<int, AbstractPizza>(); // Список готовящихся пицц
		private readonly Dictionary<int, AbstractPizza> ReadyPizzas = new Dictionary<int, AbstractPizza>();      // Список готовых пицц
		private readonly Mutex asyncDelay = new Mutex();
		private int orderNum = 0;

		public event Action IsClosing;
		
		private int OrdersCounter
		{	// Счётчик заказов, обнуляется после 100 (так во многих реальных заведениях)
			get
			{
				asyncDelay.WaitOne();	// Синхронизация для поочерёдного доступа потоков к счётчику
				{
					if (orderNum++ >= 100)
					{
						orderNum = 0;
					}
				}
				asyncDelay.ReleaseMutex();

				return orderNum;
			}
		}

		public async void Order(Pizzas pizza, Action<Ticket> TicketHandOver)
		{	// Пункт приёма заказов. 

			int orderNumber = OrdersCounter;

			// Уведомление кухни о поступлении заказа:
			CookingProcess cookingProcess = new CookingProcess(orderNumber, this);
			Ticket ticket = cookingProcess.GetTicket();
			ticket.Notifier += PizzaReady;

			// Вручение билетика покупателю:
			TicketHandOver(ticket);

			// Передача заказа на кухню и формирование списка готовящихся блюд:
			switch (pizza)
			{
				case (Pizzas.Hawaii):
					PizzaHawaii hpizza = new PizzaHawaii();
					PizzasInProcecss.Add(orderNumber, hpizza);
					await Task.Run(() => cookingProcess.Cook(hpizza));
					break;

				case (Pizzas.Neapol):
					PizzaNeapol npizza = new PizzaNeapol();
					PizzasInProcecss.Add(orderNumber, npizza);
					await Task.Run(() => cookingProcess.Cook(npizza));
					break;

				case (Pizzas.Peperoni):
					PizzaPeperoni ppizza = new PizzaPeperoni();
					PizzasInProcecss.Add(orderNumber, ppizza);
					await Task.Run(() => cookingProcess.Cook(ppizza));
					break;

				case (Pizzas.Margaret):
					PizzaMargareth mpizza = new PizzaMargareth();
					PizzasInProcecss.Add(orderNumber, mpizza);
					await Task.Run(() => cookingProcess.Cook(mpizza));
					break;

				case (Pizzas.Imperial):
					PizzaImperial ipizza = new PizzaImperial();
					PizzasInProcecss.Add(orderNumber, ipizza);
					await Task.Run(() => cookingProcess.Cook(ipizza));
					break;
			}
		}

		public AbstractPizza GetOrder(int num)
		{   // Пункт выдачи заказов:
			AbstractPizza temp;
			asyncDelay.WaitOne(1);	// Синхронизация для поочерёдного доступа потоков к словарям
			{
				temp = ReadyPizzas[num];
				ReadyPizzas.Remove(num);
			}
			asyncDelay.ReleaseMutex();

			return temp;
		}

		public string GetPizzasList()
		{   // Метод возвращающий список пицц в процессе приготовления
			return string.Join(", ", PizzasInProcecss.Values.Select(value => value.Name));
		}

		public void ClosingTIme()
		{   // Метод для делегата, оповещающего потоки приготовления о закрытии заведения
			IsClosing?.Invoke();
		}

		private void PizzaReady(int num)
		{   //	Оповещение с кухни о том, что пицца готова (метод подписываемый на эвент)
			asyncDelay.WaitOne();   // Синхронизация для поочерёдного доступа потоков к словарям
			{
				ReadyPizzas.Add(num, PizzasInProcecss[num]);
				PizzasInProcecss.Remove(num);
			}
			asyncDelay.ReleaseMutex();
		}
	}
}
