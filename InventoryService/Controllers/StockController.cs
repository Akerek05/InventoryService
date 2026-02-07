using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Végpont: Új mozgás rögzítése (Bevét / Kiadás)
        // POST: api/stock/movements
        [HttpPost("movements")]
        public async Task<IActionResult> CreateMovement([FromBody] CreateMovementDto dto)
        {
            // A) VALIDÁCIÓ: Létezik az anyag?
            var material = await _context.Materials.FindAsync(dto.MaterialId);
            if (material == null)
            {
                return NotFound("A megadott anyag nem található.");
            }

            // B) Átalakítjuk az int típust Enum-ra, és ellenőrizzük, hogy érvényes-e
            if (!Enum.IsDefined(typeof(MovementType), dto.TypeId))
            {
                return BadRequest("Érvénytelen mozgástípus! Csak 1 (Bevét) vagy 2 (Kiadás) értékek engedélyezettek.");
            }
            var movementType = (MovementType)dto.TypeId;

            // C) KRITIKUS ÜZLETI LOGIKA: Fedezet ellenőrzése kiadásnál
            if (movementType == MovementType.Out)
            {
                // Kiszámoljuk a jelenlegi készletet az eddigi mozgásokból
                // (Összes BEVÉT - Összes KIADÁS)
                // Lekérjük a mozgásokat a memóriába
                var movements = await _context.StockMovements
                    .Where(m => m.MaterialId == dto.MaterialId)
                    .ToListAsync();

               
                var currentStock = movements.Sum(m => m.Type == MovementType.In ? m.Amount : -m.Amount);

                if (currentStock < dto.Amount)
                {
                    return BadRequest($"Nincs elég fedezet! Jelenlegi készlet: {currentStock} {material.Unit}, Kiadni kívánt: {dto.Amount} {material.Unit}");
                }
            }

            // D) Mentés
            var movement = new StockMovement
            {
                MaterialId = dto.MaterialId,
                Type = movementType,
                Amount = dto.Amount,
                CreatedAt = DateTime.UtcNow
            };

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Sikeres rögzítés", MovementId = movement.Id });
        }

        // 2. Végpont: Aktuális készlet lekérdezése egy anyagra
        // GET: api/stock/{materialId}
        [HttpGet("{materialId}")]
        public async Task<IActionResult> GetCurrentStock(int materialId)
        {
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null) return NotFound("Anyag nem található.");

            // Ugyanaz a logika: (Összes BEVÉT - Összes KIADÁS)
            var movements = await _context.StockMovements
                .Where(m => m.MaterialId == materialId)
                .ToListAsync();

            var currentStock = movements.Sum(m => m.Type == MovementType.In ? m.Amount : -m.Amount);

            return Ok(new
            {
                Material = material.Name,
                CurrentStock = currentStock,
                Unit = material.Unit
            });
        }
    }
}