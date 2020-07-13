
namespace PizzaTime
{
	class PizzaPeperoni : AbstractPizza
	{
		public PizzaPeperoni()
		{
			Name = Pizzas.Peperoni.ToString();
			EatingTime = Rand.GetRandom(3, 7);
			CookingTime = Rand.GetRandom(2, 5);
		}
	}
}
