namespace InventoryApi.Models
{
    // Ez egy "enum", ami rögzíti, hogy csak kétféle mozgás lehet
    public enum MovementType
    {
        In = 1,  // Bevét
        Out = 2  // Kiadás
    }

    public class StockMovement
    {
        public int Id { get; set; }

        // Kapcsolat az anyaggal (Foreign Key)
        public int MaterialId { get; set; }

        public MovementType Type { get; set; } // In vagy Out

        public decimal Amount { get; set; } // Mennyiség

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Mikor történt?

        
    }
}