using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Outputlib;
using ConsoleDraw;

namespace SnakeGame
{
	delegate void GameOver();	// Делегат, сквозь который змейка сообщает основному процессу о смерти
	delegate void SetStats(int[] stats);	// Делегат, сквозь который змейка сообщает основному процессу статистику
	delegate void ObjectDeath(AbstractUnit unit);   // Делегат, сквозь который юниты сообщают основному процессу о смерти
	public class Program
	{
		static string name = " СНАКА 0.1 ";
		static Draw draw = new Draw(name);
		static void Main()
		{
			bool exit = false;
			int fieldLength = 117;
			while (!exit)
			{
				// Отображаем главное меню игры:
				

				string[] strings = {$"{ASCIIART}",
									"К вводу допускаются только цифры!",
									"\n Введите 0: Выход\n Введите 1: Смена ширины поля\n Введите 2-100: Желаемая скорость для начала игры. Рекомендуемая скорость 50!"};
				int input = Int32.Parse(draw.Form(new int[] { 0, 0, 100 }, strings));   // Запрашиваем у игрока желаемую скорость

				switch (input)
				{
					case 0:
						exit = true; break;
					case 1:
						fieldLength = SetFieldLenght();
						break;
					default:
						// Генерируем новый буффер отрисовки и заполняем его непечатаемыми символами:
						char[,] drawBuffer = new char[27, fieldLength];
						for (int i = 0; i < drawBuffer.GetLength(0); i++)
						{
							for (int j = 0; j < drawBuffer.GetLength(1); j++)
							{
								drawBuffer[i, j] = '!';
							}
						}
						// Передаём сгенерированый буффер отрисовки и скорость, запрошенную с клавиатуры, в новый объект игрового процесса
						Runtime runtime = new Runtime(drawBuffer, SpeedCalculator(input), name);
						runtime.Run();
						break;
				}

				Console.Clear();
			}
		}

		static int SetFieldLenght()
		{
			string[] strings = {$"Введите желаемую ширину поля (50-117, стандарт 117):",
								 "К вводу допускаются только числа 50-117:"};
			return Int32.Parse(draw.Form(new int[] { 0, 50, 117 }, strings));   // Запрашиваем у игрока желаемую скорость
		}

		static public int SpeedCalculator(int input)
		{	// Вспомогательный метод для перевода скорости из процентов в секунды задержки

			return (int)Math.Round(300 - input * 2.5);
		}
		// Дальше можно не смотреть)

















		static readonly string ASCIIART = "MMMMMMMMMMMMMMMMMMMMMMMMMMMMWX0kdlc;,,,,;:lx0XWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMMMMMMMMMMMMMMMMMWXOxoc;,''''''''''',;cox0NWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMMMMMMMMMMMMMWX0xl;,''',;:clxxd:''''''''',cd0NMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMMMMMMMMMMNKko:,'',;:clodxxxxo;'',;::;;,'''',ckXMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMMMMMMWN0xl;''';lxOkxxxxxxxl;'';lx00d.lxo;'''',cONMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMMMWNOdc,'',;:lkXWWN0kxxxxo;'';xKKKKk.cOKx:''''';dXMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMMMWKxc,'';clodxxxkKNWWXOxxxl,''l0KKKK0o.dKKo''','',dKNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMMWKo;'',:d0XKOxxxxxx0NWWXOxxl,''cOXKXXKk.cO0l'',:;'':o0WMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMMNx;'',:ox0NWWX0xxxxxxOXWWNOxd:'',lOKXXK0dcdo,'':oc'',:xWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMMNd,'':odxxxOXWWNKkxxxxxOXWWNOxo:,'';ldxkxdc;'',:odc,',;xWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM\n" +
										  " MMMWk;'':dxxxxxxkKNWWXOxxxxxONWWXkxdl:;,''','''',:ldxdc'',;xWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWNNXKKXNWMMMMMMMMM\n" +
										  " MMMNo'',lxxxxxxxxx0XWWN0kxxxx0NWNKkk0K0kdol:::clodxxxd:'';cOWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWXxl:;;;;:okKWMMMMMM\n" +
										  " MMMNd'',lxxxxxxxxxxkKWWNKOxxxOXNXK00OOkkkkdlcoxxxkkO0k:''cxKMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWXOo:'''''':kNMMMMM\n" +
										  " MMMWk;'':dxxxxxxxxxxk0KK0xlccccc::;;,,,''''''lO0KXNWNk;',dKNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMW0c''','',xNMMMM\n" +
										  " MMMMXl'',lxxxxxddolc:;;;,'''''''''''';clloodd0NWWWNXOc'':OWWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWx,',:;'':OWMMM\n" +
										  " MMMMWO;'';oddoc:;,''',;:clllc:;,'''',dKXXNNWWNXK0Okxl,''oXMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMNd'',cc,',dWMMM\n" +
										  " MMMMMWk:'',::,'',;coddddol:;,''',''':ONNXXK0Okkxxxxd;'':OMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWO:'':ol,''oNMMM\n" +
										  " MMMMMWNOc''''''';clc:;,''',;coxkl,',dKNXKkxxkkO0KXKo,',dNMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMW0c'':kKx;''oNMMM\n" +
										  " MMWKxdlc;,''''''''',;:codk0XNWWKc'':ONNXXKKXNNWWWNO:''lKMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMW0c'';xXNk;',dWMMM\n" +
										  " MMNd,''''''',:xOOOO0KXNWMMMMMMNd,', dXNNNWWWNNXXK0x:'':OWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMW0c'',lxkkl'';kWMM\n" +
										  " MMNx,''',:odkKWMMMMMMMMMMMMMMWO;''l0NXK00OOOkkxxl;'';kWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMXo'';dOOko;''cKMMMM\n" +
										  " NKOl,''',dNWMMMMMMMMMMMMMMMMMKl'':kXX0xxxxxxxxdl,'':kWMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMWk;''oXWNXd,',xWMMMM";
	}
}


