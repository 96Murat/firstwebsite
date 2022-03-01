using Microsoft.EntityFrameworkCore;
using MyProje.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyProje.BusinessLogic
{
    public class Context : DbContext
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=DESKTOP-8SH0SRV;Initial Catalog=MyProject;Integrated Security=True;");
        //}
        public Context(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<CityEntity> Citys { get; set; }
        public DbSet<DistrictEntity> Districts { get; set; }
        public DbSet<AddressEntity> UserAddresses { get; set; }
        public DbSet<UserEntity> Users { get; set; }
    }
}
