﻿using GameStore.DAL.Entities;
using System.Linq;
using GameStore.Common.Entities;

namespace GameStore.DAL.Abstract.EntityFramework
{
	public interface IEfUserRepository
	{
		User GetSingle(string userName, string password = null);

		bool Contains(string userName, string password);

		IQueryable<User> GetAll();
	}
}