﻿using GameStore.DAL.Entities;
using GameStore.DAL.Infrastructure;
using System.Linq;

namespace GameStore.DAL.Abstract.EntityFramework
{
	public interface IEfGameRepository
	{
		IQueryable<Game> GetAll(GameFilter filter = null);

		Game GetSingle(string gameKey);

		void Insert(Game game);

		void Delete(string gameKey);

		void Update(Game game);

		bool Contains(string gameKey);
	}
}