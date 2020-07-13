using System;
using System.Threading;
using System.Threading.Tasks;

namespace PizzaTime
{
	class Customer
	{   // Класс объекта покупателя


		private Ticket ticket;                  // Билетик от заказа
		private AbstractPizza pizza;            // Контейнер для полученной пиццы
		private readonly Pizzeria pizzeria;                 // Телефон пиццерии
		private readonly Pizzas orderedPizza;				// Заказанная пица
		private readonly Action<Customer> goingOut;			// Делегат, в который клиент принимает функцию отрисовщика, которую оповещает если хочет уйти
		private readonly Action<Customer> servedTrigger;    // Делегат, в который клиент принимает функцию отрисовщика, которую оповещает о том что обслужен
		private readonly int num;

		public Customer(int num, Pizzeria pizzeria)
		{   // Конструктор объекта клиент: заказывает себе пиццу по выбору (рандомом)
			this.num = num;     // Номер клиента присваивается исключительно для отображения в консоли
			this.pizzeria = pizzeria;

			orderedPizza = ChoosePizza();
			pizzeria.Order(orderedPizza, GetTicket);
		}

		public Customer(int num, Action<Customer> servedTrigger, Action<Customer> goingOut, Pizzeria pizzeria) : this(num, pizzeria)
		{   // Конструктор предназначеный для вызова из класса симуляции, добавляет делегаты-триггеры необходимые процессу симуляции
			this.goingOut = goingOut;
			this.servedTrigger = servedTrigger;
		}

		public string State { get; private set; }

		public async void GoingOut()
		{	// Асинхронный метод, запускающий процесс поедания пищи с последующим оповещением о уходе

			await Task.Run(() => GoingOutAsync());
		}

		private void GetTicket(Ticket ticket)
		{   // Метод, принимающий билетик с номером заказа (и эвентом, на который происходит подписка)

			this.ticket = ticket;
			this.ticket.Notifier += GetOrder;   // Подписка на эвент о готовности пиццы

			State = $"Покупатель {num,-3} заказал пиццу-{orderedPizza,-8} и ожидает заказ №{ticket.TicketNumber,-3}\n";
		}

		private void GetOrder(int num)
		{   // Метод взаимодействия клиента с пиццерией: клиент забирает уже готовую пиццу

			ticket.Notifier -= GetOrder;    // Отписка от эвента, чтобы на него не сохранялось ссылок
			pizza = pizzeria.GetOrder(num);
			State = $"Покупатель {this.num,-3} забрал заказ №{ticket}" +
					$" и наслаждается пиццей {pizza}\n";

			if (servedTrigger != null)
			{
				servedTrigger(this);            // Оповещение окну отрисовки о том, что клиент обслужен
			}
			else
			{	// Оповещение об успешном завершении процесса при прямом создании класса без процесса симуляции:
				Console.WriteLine($"Пицца {pizza} получена!");
			}
		}

		private Pizzas ChoosePizza()
		{   // Вспомогательный метод осущствляющий рандомный выбор пиццы

			switch (Rand.GetRandom(1, 6))
			{
				case (1):
					return Pizzas.Hawaii;
				case (2):
					return Pizzas.Neapol;
				case (3):
					return Pizzas.Peperoni;
				case (4):
					return Pizzas.Margaret;
				case (5):
					return Pizzas.Imperial;
				default:
					return Pizzas.None;
			}
		}

		private void GoingOutAsync()
		{	// Клиент кушает 3 секунды и уходит

			Thread.Sleep(TimeSpan.FromSeconds(pizza.EatingTime));

			if (goingOut != null)
			{
				goingOut(this);
			}
		}


	}
}
