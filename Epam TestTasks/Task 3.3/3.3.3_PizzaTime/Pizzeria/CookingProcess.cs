using System;
using System.Threading;

namespace PizzaTime
{
	class CookingProcess
	{
		private readonly Ticket ticket;
		private bool isClosing = false;		// Флаг о завершении работызаведения, чтобы завершать готовку в асинхронных процессах

		public CookingProcess(int number, Pizzeria pizzeria)
		{
			pizzeria.IsClosing += IsClosing;	// Подписка на оповещение о закрытии
			ticket = new Ticket(number);		// Кухня формирует талончик с номером заказа
		}

		public Ticket GetTicket()
		{	// Метод передающий талончик пользователю

			return ticket;
		}

		private void IsClosing()
		{	// Метод оповещаемый эвентом при закрытии заведения

			isClosing = true;
		}

		public void Cook<T>(T pizza) where T: AbstractPizza
		{	// Процесс готовки, готовит пицу оглядываясь на флаг закрытия заведения (для завершения асинхронного процесса)

			int count = pizza.CookingTime * 2;

			while (!isClosing && count > 0)
			{	// Ожиданее выполнено циклом по 0.5секунды а не общим временем ожидания, чтобы можно было немедленно завершить процесс

				if (isClosing)
				{
					break;
				}

				Thread.Sleep(TimeSpan.FromSeconds(0.5));
				count--;
			}

			ticket.PizzaReady();    // Оповещение пиццерию и клиента о том, что пицца готова.
		}                           // Пиццерия переместит пиццу в список готовой продукции, а клиент пойдёт её забирать
	}
}
