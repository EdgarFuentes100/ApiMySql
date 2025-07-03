using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiWeb.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<Empleado> Empleado { get; set; }
    }
}
