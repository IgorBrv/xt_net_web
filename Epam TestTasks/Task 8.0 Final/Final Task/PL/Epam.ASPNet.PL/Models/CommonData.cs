using Epam.CommonEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using Epam.DependencyResolver;

namespace Epam.ASPNet.PL.Models
{	// Пространство общих переменных PL
	public static class CommonData
	{
		public static int? idChat = null;
		public static bool authError = false;
		public static Exception commonException = null;
		public static List<UserData> curUserFriends = null;
		public static List<UserData> friendRequests = null;
		public static List<UserData> friendInventations = null;

		private static Resolver resolver;
		private static UserData currentUser = null;

		public static Resolver GetResolver() => resolver ?? (resolver = new Resolver());

		public static UserData CurrentUser 
		{
			get => currentUser;
			set
			{
				GetResolver();
				currentUser = value;
				curUserFriends = resolver.GetBloFriends.GetFriends((int)currentUser.id).ToList();
				friendRequests = resolver.GetBloFriends.GetFriendRequests((int)currentUser.id).ToList();
				friendInventations = resolver.GetBloFriends.GetInventations((int)currentUser.id).ToList();
			}
		}

		public static int AgeCalc(DateTime date)
		{
			return (DateTime.Now.Year - date.Year - 1) + (((DateTime.Now.Month > date.Month) || ((DateTime.Now.Month == date.Month) && (DateTime.Now.Day >= date.Day))) ? 1 : 0);
		}
	}
}