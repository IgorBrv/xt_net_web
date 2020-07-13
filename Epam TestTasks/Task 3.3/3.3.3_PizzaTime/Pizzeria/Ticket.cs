
namespace PizzaTime
{
	public delegate void Notify(int num);
	class Ticket
	{
		public event Notify Notifier;

		public Ticket(int num)
		{
			TicketNumber = num;
		}

		public void PizzaReady()
		{
			Notifier?.Invoke(TicketNumber);
		}

		public int TicketNumber { get; private set; }

		public override string ToString()
		{
			return TicketNumber.ToString().PadLeft(3);
		}
	}
}
