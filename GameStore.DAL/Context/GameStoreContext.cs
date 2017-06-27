using GameStore.DAL.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace GameStore.DAL.Context
{
    public class GameStoreContext : DbContext
    {
        public GameStoreContext(string connectionString)
            : base(connectionString)
        {
        }

        static GameStoreContext()
        {
			Database.SetInitializer<GameStoreContext>(new GameStoreContextInitializer());
        }

		public DbSet<Game> Games { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Genre> Genres { get; set; }

        public DbSet<PlatformType> PlatformTypes { get; set; }
    }
}