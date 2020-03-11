using System;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace DAL
{
    public class MySqlContext : DbContext
    {
        public Microsoft.EntityFrameworkCore.DbSet<Room> Rooms { get; set; }
        
        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base(options)
        { }
    }
}