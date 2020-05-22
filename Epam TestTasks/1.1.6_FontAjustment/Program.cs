using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontAjustment
{ // Предложите способ хранения информации о форматировании текста надписи и напишите программу, которая позволяет устанавливать и изменять начертание.
	class Program
	{  
		static void Main(string[] args)
		{   // !!!!! Ниже приведено 2 решение, с использованием enum и словаря Dictionary. Мне больше нравится второе. 
			string str;
			byte num = 0;
			bool error = false;
			//Font_Args_Enum font = new Font_Args_Enum();
			Font_Args_List font = new Font_Args_List();
			Console.Clear();
			while (true)
			{
				Console.BackgroundColor = ConsoleColor.Green;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($"\n ПРОГРАММА, КОТОРАЯ ПОЗВОЛЯЕТ УСТАНАВЛИВАТЬ И ИЗМЕНЯТЬ НАЧЕРТАНИЕ ТЕКСТА НАДПИСИ \n\n"); ; ;
				Console.ResetColor();

				Console.BackgroundColor = ConsoleColor.Cyan;
				Console.ForegroundColor = ConsoleColor.Black;
				Console.WriteLine($" Параметры надписи: {font.GetState()} ");
				Console.ResetColor();

				//Введём несколько уточняющих принтов отображающихся в случае некорректного ввода
				if (error)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("К вводу допускаются ТОЛЬКО числа 0, 1, 2, 3! \n");
					Console.ResetColor();
					error = false;
				}
				else Console.WriteLine("");
				// Проведём проверку ввода, и, в случае прохождения передадим команду на изменение атрибутов в экземпляр класса font.
				Console.WriteLine($"Введите желаемый атрибут (или 'exit' для выхода'):\n\n\t1:" +
					$" {Font_Args_Enum.FontAtr.Bold}\n\t2: {Font_Args_Enum.FontAtr.Itallic}\n\t3: {Font_Args_Enum.FontAtr.Underline}");
				str = Console.ReadLine().Trim().ToLower();
				if (str == "exit") break; 
				if (str.Length != 1) { error = true; };
				if (byte.TryParse(str, out num) && num < 4) font.Set(num); else error = true;
				Console.Clear();
			}
		}
	}
	class Font_Args_List
	{
		private Dictionary<byte, string> args = new Dictionary<byte, string> { { 0, "Null" }, { 1, "Bold" }, { 2, "Itallic" }, { 3, "Underline" } };
		private List<byte> state = new List<byte> { 0 };

		public void Set(byte input)
		{
			switch (input)
			{
				case 0:
					state.Clear();
					state.Add(0);
					break;
				case 1:
					if (state.Contains(1)) { state.Remove(1); }
					else { state.Add(1); if (state.Contains(0)) { state.Remove(0); }; }
					break;
				case 2:
					if (state.Contains(2)) { state.Remove(2); }
					else { state.Add(2); if (state.Contains(0)) { state.Remove(0); }; }
					break;
				case 3:
					if (state.Contains(3)) { state.Remove(3); }
					else { state.Add(3); if (state.Contains(0)) { state.Remove(0); }; }
					break;
			}
			if (state.Count == 0) state.Add(0);
			state.Sort();
		}

		public string GetState()
		{
			List<string> str = new List<string>();
			foreach (byte i in state) str.Add(args[i]);
			return string.Join(", ", str);
		}

	}
	class Font_Args_Enum
	{
		[Flags]
		public enum FontAtr
		{
			None = 1,
			Bold = 2,
			Itallic = 4,
			Underline = 8,
		}
		private FontAtr state = FontAtr.None;  // Переменная, хранящая состояние
		public string GetState()
		{  // Метод, который возвращает состояние			
			return state.ToString();
		}
		public void Set(byte input)
		{  // Метод, который устанавливает выбранный флаг
			switch (input)
			{
				case 0:
					state = FontAtr.None;
					break;
				case 1:
					if (state.HasFlag(FontAtr.Bold)) { state ^= FontAtr.Bold; }
					else { state |= FontAtr.Bold; if (state.HasFlag(FontAtr.None)) { state ^= FontAtr.None; }; }
					break;
				case 2:
					if (state.HasFlag(FontAtr.Itallic)) { state ^= FontAtr.Itallic; }
					else { state |= FontAtr.Itallic; if (state.HasFlag(FontAtr.None)) { state ^= FontAtr.None; }; }
					break;
				case 3:
					if (state.HasFlag(FontAtr.Underline)) { state ^= FontAtr.Underline; }
					else { state |= FontAtr.Underline; if (state.HasFlag(FontAtr.None)) { state ^= FontAtr.None; }; }
					break;
			}
			if (state.ToString() == "0") { state |= FontAtr.None; }
		}
	}
}
