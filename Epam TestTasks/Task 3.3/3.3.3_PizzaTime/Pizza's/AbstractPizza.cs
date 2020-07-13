
namespace PizzaTime
{
	class AbstractPizza
	{	// Упаковка для пиццы. На упаковке нанесено название, время приготовление и калорийность (время съедания)

		public string Name { get; protected set; }
		public int EatingTime { get; protected set; }
		public int CookingTime { get; protected set; }

		public override string ToString()
		{
			return Name.ToString().PadRight(8);
		}
	}
}
