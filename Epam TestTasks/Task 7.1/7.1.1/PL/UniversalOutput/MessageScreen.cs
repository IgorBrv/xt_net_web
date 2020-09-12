using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outputlib;

namespace Outputlib
{
	public static class MessageScreen
	{
		public static void CreateMessage(string header, string information = null, string footer = "нажмите Enter", bool errorMsg = true)
		{
			Console.CursorVisible = false;
			Console.Clear();
			
			int leftOffset = (int)(Math.Round(60 - (double)(header.Length / 2)));

			if (leftOffset < 0)
			{
				leftOffset = 0;
				header = $"{header.Take(115)}...";
			}

			Console.SetCursorPosition(leftOffset, 13);

			if (errorMsg)
			{
				Output.Print("b", "r", header);
			}
			else
			{
				Output.Print("b", "c", header);
			}

			if (information != null)
			{
				leftOffset = (int)(Math.Round(60 - (double)(information.Length / 2)));
				if (leftOffset < 0)
				{
					leftOffset = 0;
					information = $"{information.Take(115)}...";
				}

				Console.SetCursorPosition(leftOffset, 14);
				Output.Print("w", "", $"{information}");
			}

			leftOffset = (int)(Math.Round(60 - (double)(footer.Length / 2)));
			if (leftOffset < 0)
			{
				leftOffset = 0;
				footer = $"{footer.Take(115)}...";
			}

			Console.SetCursorPosition(leftOffset, 27);
			Output.Print("w", "", footer);
			Console.ReadKey();
			Console.CursorVisible = true;
		}
	}
}
