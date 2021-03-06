﻿using System;

namespace Entities
{	// Общие классы

	public class User
	{   // Класс объекта пользователя

		public Guid id;
		public int age;
		public string name;
		public DateTime birth;
		public string emblempath;

		public User(Guid id, int age, string name, DateTime birth, string emblempath = null)
		{
			this.id = id;
			this.age = age;
			this.name = name;
			this.birth = birth;
			this.emblempath = emblempath;
		}
	}
}
