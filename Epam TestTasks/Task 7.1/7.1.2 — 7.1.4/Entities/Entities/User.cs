﻿using System;

namespace Entities
{	// Общие классы

	public class User : IHaveID
	{	// Класс объекта пользователя

		public int age;
		public string name;
		public DateTime birth;

		public User(Guid id, int age, string name, DateTime birth)
		{
			this.id = id;
			this.age = age;
			this.name = name;
			this.birth = birth;
		}
		public Guid id { get; set; }
	}
}
