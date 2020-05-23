using System;

namespace Outputlib
{
	public class Output
	{
		static public void Print(string fnt = "w", string bck = "b", bool write = true, params string[] strings)
		{
			Draw(fnt, bck, write, strings);
		}

		static public void Print(string fnt = "w", string bck = "b", params string[] strings)
		{
			Draw(fnt, bck, true, strings);
		}

		static private void Draw(string fnt = "w", string bck = "b", bool write = true, params string[] strings)
		{
			ConsoleColor font = new ConsoleColor();
			ConsoleColor background = new ConsoleColor();

			switch (fnt)
			{
				case "b":
					font = ConsoleColor.Black; break;

				case "r":
					font = ConsoleColor.Red; break;
			}
			switch (bck)
			{
				case "r":
					background = ConsoleColor.Red; break;
				case "y":
					background = ConsoleColor.Yellow; break;
				case "c":
					background = ConsoleColor.Cyan; break;
				case "g":
					background = ConsoleColor.Green; break;
				case "db":
					background = ConsoleColor.DarkBlue; break;
			}

			Console.BackgroundColor = background;
			Console.ForegroundColor = font;
			if (write) { foreach (string i in strings) Console.WriteLine(i); }
			else	   { foreach (string i in strings) Console.Write(i);     }
			Console.ResetColor();
		}
	}
}
