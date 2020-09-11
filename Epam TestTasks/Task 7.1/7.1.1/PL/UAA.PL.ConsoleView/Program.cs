using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLLInterface;
using BLLConnector;
using PLInterface;
using Entities;

namespace PLConsole
{
	public class PLConsole : IPl
	{
		private readonly IBll bll;
		static void Main()
		{
			new PLConsole();
			Console.WriteLine("!!!");
			Console.ReadLine();
		}

		public PLConsole()
		{
			this.bll = BLLGetter.GetBll();
			this.bll.SetPL(this);
			bll.AddUser("Вася", 38, new DateTime(1999, 12, 2));
			List<User> users = bll.GetAllUsers();

			Console.WriteLine(users.FindIndex(user => user.id == Guid.NewGuid()));
			foreach (User user in users)
			{
				Console.WriteLine($"{user.name}, {user.age}, {user.birth}");
				bll.UpdateUser(user.id, "петро", user.age, user.birth);
				//Console.WriteLine(bll.RemoveUser(user));
			}
		}
	}
}
