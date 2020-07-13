
namespace PizzaTime
{
	class PizzaNeapol : AbstractPizza
	{
		public PizzaNeapol()
		{
			Name = Pizzas.Neapol.ToString();
			EatingTime = Rand.GetRandom(3, 6);
			CookingTime = Rand.GetRandom(9, 12);
		}
	}
}
