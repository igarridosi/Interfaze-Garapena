using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPV_Sistema.Models;

public class ElkarteaDbContext : DbContext
{
    // Propietate bakoitza datu-baseko taula bat bihurtuko da
    public DbSet<Erabiltzailea> Erabiltzaileak { get; set; }
    public DbSet<Produktua> Produktuak { get; set; }
    public DbSet<Mahaia> Mahaiak { get; set; }
    public DbSet<Erreserba> Erreserbak { get; set; }
    public DbSet<Eskaera> Eskaerak { get; set; }
    public DbSet<EskaeraLerroa> EskaeraLerroak { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=elkartea.db");
    }
}
