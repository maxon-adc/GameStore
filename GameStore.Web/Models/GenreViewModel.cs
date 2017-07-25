﻿using GameStore.Services.DTOs;
using System.Collections.Generic;

namespace GameStore.Web.Models
{
	public class GenreViewModel
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public List<GenreViewModel> ChildGenres { get; set; }

		public GenreDto ParentGenre { get; set; }
	}
}