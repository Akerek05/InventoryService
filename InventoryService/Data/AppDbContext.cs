using InventoryApi.Models; // Hogy lássa a modelleket
using Microsoft.EntityFrameworkCore; // Hogy lássa a DbContext-et

namespace InventoryApi.Data
{
    public class AppDbContext : DbContext
    {
        // Konstruktor: Ezen keresztül kapja meg a beállításokat (pl. hogy SQLite-ot használunk)
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ez hozza létre a "Materials" táblát
        public DbSet<Material> Materials { get; set; }

        // Ez hozza létre a "StockMovements" táblát
        public DbSet<StockMovement> StockMovements { get; set; }

    }
}