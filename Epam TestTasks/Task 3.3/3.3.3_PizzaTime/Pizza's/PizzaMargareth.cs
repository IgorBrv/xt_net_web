
namespace PizzaTime
{
	class PizzaMargareth: AbstractPizza
	{
		public PizzaMargareth()
		{
			Name = Pizzas.Margaret.ToString();
			EatingTime = Rand.GetRandom(3, 6);
			CookingTime = Rand.GetRandom(7, 15);
		}
	}
}
