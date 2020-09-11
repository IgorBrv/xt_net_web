using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterfaceBLL;
using Entities;
using Dependencies;

namespace ConsoleViewPL
{
	public class PLConsole
	{
		private readonly IBll bll;
		static void Main()
		{
			new PLConsole();
			Console.ReadLine();
		}

		public PLConsole()
		{
			bll = Resolver.GetBLL;
			bll.AddUser("Вася", 38, new DateTime(1999, 12, 2));
			List<User> users = bll.GetAllUsers();

			foreach (User user in users)
			{
				Console.WriteLine($"{user.name}, {user.age}, {user.birth}");
				bll.UpdateUser(user.id, "Петя", user.age, user.birth);
				Console.WriteLine(bll.RemoveUser(user));
			}
		}
	}
}
