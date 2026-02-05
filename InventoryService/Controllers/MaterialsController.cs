using InventoryApi.Data;
using InventoryApi.DTOs;
using InventoryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Itt kapjuk meg az adatbázis-kapcsolatot (Dependency Injection)
        public MaterialsController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Végpont: Minden anyag listázása
        // GET: api/materials
        [HttpGet]
        public async Task<ActionResult<List<Material>>> GetMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        // 2. Végpont: Új anyag felvétele
        // POST: api/materials
        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial(CreateMaterialDto dto)
        {
            // 1. DUPLIKÁCIÓ ELLENŐRZÉS
            // Megnézzük, van-e már ilyen nevű ÉS mértékegységű anyag az adatbázisban
            bool exists = await _context.Materials.AnyAsync(m => m.Name == dto.Name && m.Unit == dto.Unit);

            if (exists)
            {
                // 409 Conflict hibát adunk vissza, ami jelzi, hogy az adat már létezik
                return Conflict($"Már létezik anyag ezzel a névvel és mértékegységgel: {dto.Name} ({dto.Unit})");
            }

            // 2. Ha nincs, akkor létrehozzuk
            var material = new Material
            {
                Name = dto.Name,
                Unit = dto.Unit
            };

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterials), new { id = material.Id }, material);
        }
    }
}