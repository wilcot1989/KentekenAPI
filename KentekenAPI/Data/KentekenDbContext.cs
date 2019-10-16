using Microsoft.EntityFrameworkCore;
using KentekenAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KentekenAPI.Data
{
    public class KentekenDbContext : DbContext
    {
        public KentekenDbContext(DbContextOptions<KentekenDbContext> options)
            :base(options)
        {
        }

        public DbSet<KentekenInfo> KentekenInfo { get; set; }
        public DbSet<UserKentekenLog> UserKentekenLog { get; set; }
    }
}
