using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CBoxController : ControllerBase
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly CAppDbContext _context;
        public CBoxController(CAppDbContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CBox>>> GetAll() => await _context.CBoxes.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<CBox>> GetById(long id)
        {
            var box = await _context.CBoxes.FindAsync(id);
            return box is null ? NotFound() : Ok(box);
        }

        [HttpPost]
        public async Task<ActionResult<CBox>> Create(CBox box)
        {
            _context.CBoxes.Add(box);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = box._Key }, box);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, CBox updatedBox)
        {
            if (id != updatedBox._Key) return BadRequest();
            _context.Entry(updatedBox).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var box = await _context.CBoxes.FindAsync(id);
            if (box is null) return NotFound();

            _context.CBoxes.Remove(box);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
