﻿namespace Task_3_1_1
{
	class Human
	{	// Класс описывающий участника "Слабого звена
		private string name;
		private bool active = true;
		public bool Active
		{
			get
			{
				return active;
			}
			set
			{
				if (!value)
				{
					name = $"[cl][rb]{name}";
					active = value;
				}
				else
				{
					active = value;
				}
			}
		}	

		public Human(int num)
		{
			name = $"Человек {num}\n";
		}

		public override string ToString()
		{	// Переопределение метода ToString для последующего использования в конструкции string.Join, для выведения имен всех участников ввиде строки.
			return name;
		}
	}
}
