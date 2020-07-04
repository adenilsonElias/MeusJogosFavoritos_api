using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MeusJogosFavoritos.model
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) {
            Database.EnsureCreated();
            Database.Migrate();
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder){
            modelBuilder.Entity<User_Games>().HasKey(t => new { t.id_User , t.id_game });
            modelBuilder.Entity<Amigo>().HasKey(t => new { t.amigoId , t.userId });

            modelBuilder.Entity<User_Games>().HasOne(ug => ug.user).WithMany(user => user.favoritos).HasForeignKey(ug => ug.id_User);
            modelBuilder.Entity<User_Games>().HasOne(ug => ug.games).WithMany(games => games.favorite).HasForeignKey(ug => ug.id_game);
            modelBuilder.Entity<Amigo>().HasOne(a => a.user).WithMany(u => u.amigos).HasForeignKey(a => a.userId);
            modelBuilder.Entity<Amigo>().HasOne(a => a.amigo).WithMany(u => u.amigoDe).HasForeignKey(a => a.amigoId);
        }
        public DbSet<UserModel> users {get;set;}
        public DbSet<Games> games {get;set;}
        public DbSet<User_Games> user_games {get;set;}
        public DbSet<Amigo> amigos {get;set;}


    }
}
