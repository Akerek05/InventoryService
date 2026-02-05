namespace InventoryApi.Models
{
    public class Material
    {
        public int Id { get; set; } // Elsődleges kulcs
        public string Name { get; set; } = string.Empty; // Pl. "Cement"
        public string Unit { get; set; } = string.Empty; // Pl. "Zsák", "kg"
    }
}