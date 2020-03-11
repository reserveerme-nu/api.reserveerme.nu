using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Models;

namespace DAL
{
    public class MySqlContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        
        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base(options)
        { }
        
        public MySqlContext()
            : base()
        { }
    }
}