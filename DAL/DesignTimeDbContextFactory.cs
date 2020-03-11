using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;

namespace DAL
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        public MySqlContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MySqlContext>();
            var connectionString = configuration.GetConnectionString("MySql");
            Console.WriteLine(connectionString + "yes");
            builder.UseMySql(connectionString);
            return new MySqlContext(builder.Options);
        }
    }
}