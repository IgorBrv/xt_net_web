
namespace PizzaTime
{
	class PizzaHawaii : AbstractPizza
	{
		public PizzaHawaii()
		{
			Name = Pizzas.Hawaii.ToString();
			EatingTime = Rand.GetRandom(3, 5);
			CookingTime = Rand.GetRandom(5, 8);
		}
	}
}
