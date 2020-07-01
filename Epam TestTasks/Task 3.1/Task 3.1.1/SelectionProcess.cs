using System.Collections.Generic;
using Outputlib;

namespace Task_3_1_1
{
	class SelectionProcess
	{
		private readonly Draw draw;
		private readonly List<string> endingStrings;
		private readonly List<Human> participantsList;

		public SelectionProcess(int participants, Draw draw)
		{	// Создание объекта симуляции с учётом колличества участников
			this.draw = draw;
			participantsList = new List<Human>();	// генерация списка участников
			endingStrings = new List<string> { "\n[cl][bc]--- Оставшиеся участники:\n" };
			GenerateParticipantsList(participants, participantsList);

		}
		public void Run(int step)
		{	// Запуск процесса симуляции с учётом заданного шага
			int position = 1;
			bool exit = false;
			string headline = "Симуляция. Красным выделены выбывшие люди < Нажмите Enter >:";
			draw.Form(new int[] { 3 }, new string[] { headline, string.Empty, string.Join("", participantsList) });

			while (!exit)
			{	// Запуск цикла симуляции. Цикл построен с учётом особенностей работы универсальной библиотеки отрисовки OutputLib
				int active_count = 0;
				for (int i = 0; i < participantsList.Count; i++)
				{
					if (participantsList[i].Active)
					{
						if (position == step)
						{
							position = 1;
							participantsList[i].Active = false;
							draw.Form(new int[] { 3 }, new string[] { headline, string.Empty, string.Join("", participantsList) });
						}
						else
						{
							active_count++;
							position++;
						}
					}
				}

				if (active_count < step)
				{	// Завершение цикла при колличестве активных участников меньшем чем заданый шаг
					exit = true;
				}
			}

			foreach (Human hum in participantsList)
			{	// Добавление оставшихся участников в результирующий "список"
				if (hum.Active)
				{
					endingStrings.Add(hum);
				}
			}

			// Отрисовка финальной формы со списком оставшихся участников
			draw.Form(new int[] { 3 }, new string[] { $"Симуляция завершена. Невозможно исключить больше людей <Нажмите Enter>:", "", string.Join("", participantsList) + string.Join("", endingStrings) });

		}

		private void GenerateParticipantsList(int participants, List<Human> participantsList)
		{	// Вспомогательный метод генерирующий список участников
			for (int i = 1; i <= participants; i++)
			{
				participantsList.Add(new Human(i));
			}
		}
	}
}
