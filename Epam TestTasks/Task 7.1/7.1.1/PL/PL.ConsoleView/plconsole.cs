using System;
using System.Collections.Generic;
using System.Linq;
using InterfacesBLL;
using Entities;
using Dependencies;
using Outputlib;
using System.IO;

namespace ConsoleViewPL
{
	public class PLConsole
	{	// Консольный PL для задачи 7.1.1

		private readonly IUsersBLO bllUsers;
		private readonly IAwardsBLO bllAwards;
		private readonly IAwardsAssotiationsBLO bllAwardsAssotiations;
		private static readonly string name = " USERS AND AWARDS";
		private readonly Draw draw = new Draw(name);

		static void Main()
		{
			new PLConsole();
		}

		public PLConsole()
		{
			Resolver resolver = new Resolver("");
			bllUsers = resolver.GetBLLUsers;
			bllAwards = resolver.GetBLLAwards;
			bllAwardsAssotiations = resolver.GetBLLAwardsAssotiations;

			// Эта секция добавлена для теста:
			if (!File.Exists("notfirstrun"))
			{
				bllUsers.AddUser("Вася", 38, new DateTime(1999, 12, 2));
				bllUsers.AddUser("Коля", 28, new DateTime(1989, 5, 30));
				bllAwards.AddAward("Награда за выслугу лет");
				bllAwards.AddAward("Награда за ответственность");
				File.Create("notfirstrun");
			}

			ShowUserList();
		}


		// ДАЛЕЕ ОПИСЫВВАЕТСЯ СТРУКТУРА КОНСОЛЬНОГО МЕНЮ


		private void ShowUserList()
		{	// Меню отображения списка пользователей

			List<User> users = bllUsers.GetAllUsers();
			List<string> defaultItems = new List<string>() { "[   Создать пользователя   ]", "[ Посмотреть список наград ]", "" };
			List<string> menuItems = defaultItems.Concat(users.Select(item => item.name)).ToList();

			bool exit = false;
			int curposition = 0;

			while (!exit)
			{
				int[] feedback = NavigationMenu.CreateMenu(name, " Главное меню. Список пользователей. Для перемотки используйте клавиши вниз и вверх", menuItems, true, curposition, 2);

				switch (feedback[0])
				{
					case -1:

						exit = true;
						break;

					case 0:
						
						if (bllUsers.RemoveUser(users[feedback[1]-3]))
						{
							MessageScreen.CreateMessage(" Пользователь удалён успешно! ", $"Имя: {menuItems[feedback[1]]}", errorMsg: false);
						}
						else
						{
							MessageScreen.CreateMessage(" Не удалось удалить пользователя! ", $"Имя: {menuItems[feedback[1]]}");
						}

						users.RemoveAt(feedback[1] - 3);
						menuItems.RemoveAt(feedback[1]);
						curposition = feedback[1] - 1;
						break;

					case 1:

						if (feedback[1] == 0)
						{
							User user = CreateUser();

							if (user != null)
							{
								int curLength = menuItems.Count;
								users.Add(user);
								menuItems.Add(user.name);
								MessageScreen.CreateMessage(" Пользователь создан успешно! ", $"Имя: {menuItems[curLength]}", errorMsg: false);
							}
							else
							{
								MessageScreen.CreateMessage(" Не удалось создать пользователя! ", errorMsg: true);
							}

							curposition = 0;
						}
						else if (feedback[1] == 1)
						{
							ShowAwards();
							curposition = feedback[1];
						}
						else
						{
							ShowUser(users[feedback[1]-3]);
							users = bllUsers.GetAllUsers();
							menuItems = defaultItems.Concat(users.Select(item => item.name)).ToList();
							curposition = feedback[1];
						}
						
						break;

					default:
						curposition = feedback[1];
						break;
				}
			}
		}


		private Award ShowAwards(bool selection = false)
		{	// Меню отображения списка наград

			bool exit = false;
			int curposition = 0;
			List<string> menuItems;
			List<Award> items = bllAwards.GetAllAwards();


			if (!selection)
			{
				menuItems = new List<string>() { "[ Добавить награду ]", "" }.Concat(items.Select(item => item.title)).ToList();
			}
			else
			{
				menuItems = items.Select(item => item.title).ToList();

				if (menuItems.Count == 0)
				{
					MessageScreen.CreateMessage(" Список наград пуст! ", errorMsg: true);
					exit = true;
				}
			}

			while (!exit)
			{
				int[] feedback;

				if (!selection)
				{
					feedback = NavigationMenu.CreateMenu(name, " Меню наград. Для перемотки используйте клавиши вниз и вверх", menuItems, true, curposition, 1);
				}
				else
				{
					feedback = NavigationMenu.CreateMenu(name, " Меню наград. Для перемотки используйте клавиши вниз и вверх", menuItems, false, curposition);
				}


				switch (feedback[0])
				{
					case -1:

						exit = true;
						break;

					case 0:

						if (!selection)
						{
							if (bllAwards.RemoveAward(items[feedback[1] - 2]))
							{
								MessageScreen.CreateMessage(" Награда удалена успешно! ", $"Имя: {menuItems[feedback[1]]}", errorMsg: false);
							}
							else
							{
								MessageScreen.CreateMessage(" Не удалось удалить награду! ", $"Имя: {menuItems[feedback[1]]}");
							}
							items.RemoveAt(feedback[1] - 2);
							menuItems.RemoveAt(feedback[1]);
							curposition = feedback[1] - 1;
							break;
						}

						curposition = feedback[1];
						break;

					case 1:

						if (selection)
						{
							return items[feedback[1]];
						}

						if (feedback[1] == 0)
						{
							string awardName = draw.Form(new int[] { 2, 10 }, new string[] { "Введите назание награды (максимум 10 слов): ", "К вводу допускаютя только слова состоящие из букв!" });
							Award award = bllAwards.AddAward(awardName);

							if (award != null)
							{
								items.Add(award);
								menuItems.Add(award.title);
								MessageScreen.CreateMessage(" Награда создана успешно! ", $"Награда: '{awardName}'", errorMsg: false);
							}
							else
							{
								MessageScreen.CreateMessage(" Не удалось создать награду! ");
							}
							curposition = 0;
						}
						else
						{
							curposition = feedback[1];
							ShowAward(items[feedback[1]-2]);
						}
						
						break;

					default:

						curposition = feedback[1];
						break;
				}
			}
			return null;
		}

		private void ShowUser(User user)
		{	// Меню вывода на экран информации о пользовавтеле

			bool exit = false;
			int curposition = 0;
			List<string> items = new List<string>() { $"Имя: {user.name}", $"Возраст: {user.age}", $"Дата рождения: {user.birth:D}", "", " [ Наградить пользователя ]", "", "НАГРАДЫ ПОЛЬЗОВАТЕЛЯ:", "" };
			List<Award> usersAwards = bllAwardsAssotiations.GetAllAwardsOfUser(user);
			List<string> itemsWawards = items.Concat(usersAwards.Select(item => $"-{item.title}")).ToList();

			while (!exit)
			{
				int[] feedback = NavigationMenu.CreateMenu(name, " Меню пользователя. Для перемотки используйте клавиши вниз и вверх. Для изменения выберите желаемый пункт", itemsWawards, true, curposition, 7);

				switch (feedback[0])
				{
					case -1:

						exit = true;
						break;

					case 0:

						if (bllAwardsAssotiations.RemoveAwardFromUser(user, usersAwards[feedback[1] - 8]))
						{
							MessageScreen.CreateMessage($" У пользователя {user.name} успешно отняли награду! ", errorMsg: false);
							itemsWawards.RemoveAt(feedback[1]);
							curposition = feedback[1] - 1;

							while (itemsWawards[curposition] == "")
							{
								curposition -= 1;
							}

							break;
						}
						else
						{
							MessageScreen.CreateMessage(" Не удалось отнять награду пользователя! ");
						}

						curposition = feedback[1];
						break;

					case 1:
						if (feedback[1] < 3)
						{
							string oldName = user.name;

							if (feedback[1] == 0)
							{

								string userName = draw.Form(new int[] { 2, 3 }, new string[] { "Введите имя пользователя: ", "К вводу допускаютя только имена!" });

								if (bllUsers.UpdateUser(user.id, userName, user.age, user.birth))
								{
									MessageScreen.CreateMessage($" Имя пользователя {oldName} изменено успешно! ", $"Новое имя: {userName}", errorMsg: false);
								}
								else
								{
									MessageScreen.CreateMessage(" Не удалось изменить имя пользователя! ");
								}
							}
							else if (feedback[1] == 1)
							{
								int oldAge = user.age;
								string userAge = draw.Form(new int[] { 0, 1, 150 }, new string[] { "Введите возраст пользователя: ", "К вводу допускаютя только целые числа больше 0 и меньше 150!" });

								if (bllUsers.UpdateUser(user.id, user.name, Int32.Parse(userAge), user.birth))
								{
									MessageScreen.CreateMessage($" Возраст пользователя {oldName} изменён успешно! ", $"Новый возраст: {userAge}", errorMsg: false);
								}
								else
								{
									MessageScreen.CreateMessage(" Не удалось изменить возраст пользователя! ");
								}
							}
							else if (feedback[1] == 2)
							{
								bool dateChanged = false;
								bool showError = false;

								while (!dateChanged)
								{
									string date = draw.Form(new int[] { 1, 3, 1, DateTime.Now.Year }, new string[] { "Введите дату рождения в формате: ДЕНЬ,МЕСЯЦ,ГОД: ", "К вводу допускаютя только даты в формате: ДЕНЬ,МЕСЯЦ,ГОД!" }, showError);
									string[] dateParts = date.Split(',');

									try
									{
										DateTime birthDate = new DateTime(Int32.Parse(dateParts[2]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[0]));

										if (bllUsers.UpdateUser(user.id, user.name, user.age, birthDate))
										{
											MessageScreen.CreateMessage($" Дата рождения пользователя {user.name} изменена успешно! ", $"Новая дата: {birthDate:D}", errorMsg: false);
										}
										else
										{
											MessageScreen.CreateMessage(" Не удалось изменить дату рождения пользователя! ");
										}
										dateChanged = true;
									}
									catch (ArgumentOutOfRangeException)
									{
										showError = true;
									}
								}
							}

							user = bllUsers.GetUserByID(user.id);
							items = new List<string>() { $"Имя: {user.name}", $"Возраст: {user.age}", $" Дата рождения: {user.birth:D}", "", "[ Наградить пользователя ]", "", " НАГРАДЫ ПОЛЬЗОВАТЕЛЯ:", "" };
							itemsWawards = items.Concat(bllAwardsAssotiations.GetAllAwardsOfUser(user).Select(item => $"-{item.title}")).ToList();
						}
						else if (feedback[1] == 4)
						{
							Award award = ShowAwards(true);

							if (award != null)
							{
								if (bllAwardsAssotiations.AddAwardToUser(user, award))
								{
									MessageScreen.CreateMessage($" Пользователю {user.name} успешно добавлена награда! ", errorMsg: false);
									itemsWawards = items.Concat(bllAwardsAssotiations.GetAllAwardsOfUser(user).Select(item => $"-{item.title}")).ToList();
								}
								else
								{
									MessageScreen.CreateMessage(" Не удалось вручить награду пользователю! ");
								}
							}
						}

						curposition = feedback[1];
						break;

					default:

						break;
				}
			}
		}

		private void ShowAward(Award award)
		{	// Меню ввывода на экран информации о награде

			bool exit = false;
			int curposition = 0;
			List<string> items = new List<string>() { $"Название награды: {award.title}", "", "НАГРАЖДЁННЫЕ ПОЛЬЗОВАТЕЛИ:", "" };
			List<User> awardsUsers = bllAwardsAssotiations.GetAllUsersWithAward(award);
			List<string> itemsWawards = items.Concat(awardsUsers.Select(item => $"-{item.name}")).ToList();

			while (!exit)
			{
				int[] feedback = NavigationMenu.CreateMenu(name, " Меню награды. Для перемотки используйте клавиши вниз и вверх. Для изменения выберите название награды:", itemsWawards, false, curposition);

				switch (feedback[0])
				{
					case -1:

						exit = true;
						break;

					case 1:

						if (feedback[1] == 0)
						{

							string oldTitle = award.title;
							string awardName = draw.Form(new int[] { 2, 10 }, new string[] { "Введите новое название награды: ", "К вводу допускаютя только слова без символов!" });

							if (bllAwards.UpdateAward(award.id, awardName))
							{
								MessageScreen.CreateMessage($" Название награды '{oldTitle}' изменено успешно! ", $"Новое название: {awardName}", errorMsg: false);
								items = new List<string>() { $"Название награды: {award.title}", "", "НАГРАЖДЁННЫЕ ПОЛЬЗОВАТЕЛИ:", "" };
								itemsWawards = items.Concat(awardsUsers.Select(item => $"-{item.name}")).ToList();
							}
							else
							{
								MessageScreen.CreateMessage(" Не удалось изменить название награды! ");
							}
						}

						curposition = feedback[1];
						break;

					default:

						curposition = feedback[1];
						break;
				}
			}
		}

		private User CreateUser()
		{	// Метод реализующий создание пользователя через меню

			string userName = draw.Form(new int[] { 2, 3 }, new string[] { "Введите имя пользователя: ", "К вводу допускаютя только имена!" });
			string userAge = draw.Form(new int[] { 0, 1, 150 }, new string[] { "Введите возраст пользователя: ", "К вводу допускаютя только целые числа больше 0 и меньше 150!" });

			bool showError = false;

			while (true)
			{
				string date = draw.Form(new int[] { 1, 3, 1, DateTime.Now.Year }, new string[] { "Введите дату рождения в формате: ДЕНЬ,МЕСЯЦ,ГОД: ", "К вводу допускаютя только даты в формате: ДЕНЬ,МЕСЯЦ,ГОД!" }, showError);
				string[] dateParts = date.Split(',');

				try
				{
					DateTime birthDate = new DateTime(Int32.Parse(dateParts[2]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[0]));
					return bllUsers.AddUser(userName, Int32.Parse(userAge), birthDate);
				}
				catch (ArgumentOutOfRangeException)
				{
					showError = true;
				}
			}
		}
	}
}
