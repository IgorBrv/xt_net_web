using System;
using System.Collections.Generic;
using Outputlib;   // Кастомная библиотека вывода текста в цвете


namespace FontAjustment
{   // Предложите способ хранения информации о форматировании текста надписи и напишите программу, которая позволяет устанавливать и изменять начертание.
	class Program
	{  
		static void Main(string[] args)
		{
			string str;
			byte num = 0;
			bool error = false;
			Font_Args_Enum font = new Font_Args_Enum();
			//Font_Args_List font = new Font_Args_List();
			Console.Clear();
			while (true)
			{
				Output.Print("b", "g", $"\n ПРОГРАММА, КОТОРАЯ ПОЗВОЛЯЕТ УСТАНАВЛИВАТЬ И ИЗМЕНЯТЬ НАЧЕРТАНИЕ ТЕКСТА НАДПИСИ \n");
				Output.Print("b", "c", $"\n Параметры надписи: {font.GetState()} ");

				if (error)
				{
					Output.Print("r", "", "К вводу допускаются ТОЛЬКО числа 0, 1, 2, 3! \n");
					error = false;
				}
				else Console.WriteLine("\n");

				// Проведём проверку ввода, и, в случае прохождения передадим команду на изменение атрибутов в экземпляр класса font.
				Console.WriteLine($"Введите желаемый атрибут (или 'exit' для выхода'):\n\n\t1:" +
						          $" {Font_Args_Enum.FontAtr.Bold}\n\t2:" +
								  $" {Font_Args_Enum.FontAtr.Itallic}\n\t3:" +
								  $" {Font_Args_Enum.FontAtr.Underline}");

				str = Console.ReadLine().Trim().ToLower();
				if (str  ==  "exit") break; 
				if (str.Length != 1) error = true;
				if (byte.TryParse(str, out num) && num < 4) font.SetState(num);
				else error = true;

				Console.Clear();
			}
		}
	}
	class Font_Args_List
	{
		private bool[] state = { true, false, false, false };  // Переменная, хранящая флаги состояния
		private Dictionary<byte, string> args = new Dictionary<byte, string>
		{
			{ 0, "Null" }, { 1, "Bold" }, { 2, "Itallic" }, { 3, "Underline" }
		};

		public void SetState(byte input)
		{
			switch (input)
			{
				case 0:
					for (int i = 1; i <= 3; i++) state[i] = false;
					state[0] = true; break;
				case 1:
					state[1] = !state[1]; break;
				case 2:
					state[2] = !state[2]; break;
				case 3:
					state[3] = !state[3]; break;
			}
			byte sum = 0;
			for (int i = 1; i <= 3; i++) if (state[i]) sum+=1;
			if (sum > 0) state[0] = false;
			else state[0] = true;
		}
		public string GetState()
		{
			List<string> str = new List<string>();
			for (byte i = 0; i <= 3; i++) if (state[i]) str.Add(args[i]);
			return string.Join(", ", str);
		}
	}

	class Font_Args_Enum
	{
		[Flags]
		public enum FontAtr
		{
			None = 0,
			Bold = 1,
			Itallic = 2,
			Underline = 4,
		}
		private FontAtr state = FontAtr.None;  // Переменная хранящая состояние
		public string GetState()
		{  // Метод, который возвращает состояние			
			return state.ToString();
		}
		public void SetState(byte input)
		{  // Метод, который устанавливает выбранный флаг
			switch (input)
			{
				case 0:
					state = FontAtr.None; break;
				case 1:
					state ^= FontAtr.Bold; break;
				case 2:
					state ^= FontAtr.Itallic; break;
				case 3:
					state ^= FontAtr.Underline; break;
			}
			if (state.ToString() == "0") { state |= FontAtr.None; }
		}
	}
}
