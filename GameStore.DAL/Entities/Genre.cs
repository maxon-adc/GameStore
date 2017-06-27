﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Abstract;

namespace GameStore.DAL.Entities
{
	public class Genre : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Genre> ChildGenres { get; set; }

        public Genre ParentGenre { get; set; }

        public Genre()
        {
            ChildGenres = new List<Genre>();
        }
    }
}
