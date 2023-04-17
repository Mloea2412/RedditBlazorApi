using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using RedditProjectBlazorApi.Model;

namespace RedditProjectBlazorApi.Model
{
    //På denne side, bliver DB tabellen grundlagt, det vil sige f.eks at vi får oprettet en tabel der hedder Posts som indeholder post objekter
    // Pathen til den lokale Database bliver også defineret her
    public class RedditContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public string DbPath { get; }

        public RedditContext(DbContextOptions<RedditContext> options)
            : base(options)
        {
            DbPath = " bin/Reddit.db";
        }

        public RedditContext()
        {
            DbPath = " bin/Reddit.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().ToTable("Post");

        }
    }
}

