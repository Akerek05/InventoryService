using System.ComponentModel.DataAnnotations; // A validációhoz (pl. Required)

namespace InventoryApi.DTOs
{
    // Ezt küldi a kliens, amikor új anyagot rögzít
    public class CreateMaterialDto
    {
        [Required] // Kötelező mező
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Unit { get; set; } = string.Empty;
    }

    // Ezt küldi a kliens, amikor mozgást rögzít (bevét/kiadás)
    public class CreateMovementDto
    {
        [Required]
        public int MaterialId { get; set; }

        [Required]
        public int TypeId { get; set; } // 1 = In, 2 = Out (egyszerűsítve int-ként várjuk)

        [Range(0.01, double.MaxValue, ErrorMessage = "A mennyiségnek nagyobbnak kell lennie 0-nál.")]
        public decimal Amount { get; set; }
    }
}