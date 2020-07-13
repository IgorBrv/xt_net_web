
namespace PizzaTime
{
	class PizzaImperial : AbstractPizza
	{
		public PizzaImperial()
		{
			Name = Pizzas.Imperial.ToString();
			EatingTime = Rand.GetRandom(3, 4);
			CookingTime = Rand.GetRandom(10, 14);
		}
	}
}
