﻿using GameStore.DAL.Abstract;
using GameStore.DAL.Abstract.Common;
using GameStore.DAL.Abstract.EntityFramework;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Concrete
{
	public class GenreCopier : ICopier<Genre>
	{
		private readonly IEfGenreRepository _genreRepository;
		private readonly IUnitOfWork _unitOfWork;

		public GenreCopier(IEfGenreRepository genreRepository, IUnitOfWork unitOfWork)
		{
			_genreRepository = genreRepository;
			_unitOfWork = unitOfWork;
		}

		public Genre Copy(Genre genre)
		{
			if (!_genreRepository.Contains(genre.Name))
			{
				genre.Games?.Clear();
				_genreRepository.Insert(genre);
				_unitOfWork.Save();
			}

			return genre.Id != default(int) ? genre : _genreRepository.GetSingle(genre.Name);
		}
	}
}