﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Domain.Abstract;

namespace GameStore.Domain.Entities
{
    public class Genre : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Genre> ChildGenres { get; set; }
        public Genre ParentGenre { get; set; }
        public bool IsDeleted { get; set; }

        public Genre()
        {
            ChildGenres = new List<Genre>();
        }
    }
}
