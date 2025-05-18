using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPackController : ControllerBase
    {
        //{
        //    public IActionResult Index()
        //    {
        //        return View();
        //    }
        //}
        private readonly CAppDbContext _context;
        public CPackController(CAppDbContext context) => _context = context;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CPack>>> GetAll() => await _context.CPacks.ToListAsync();
        [HttpGet("{id}")]
        public async Task<ActionResult<CPack>> GetById(long id)
        {
            var pack = await _context.CPacks.FindAsync(id);
            return pack is null ? NotFound() : Ok(pack);
        }
        [HttpPost]
        public async Task<ActionResult<CPack>> Create(CPack pack)
        {
            _context.CPacks.Add(pack);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = pack._SeqNo }, pack);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, CPack updatedPack)
        {
            if (id != updatedPack._SeqNo) return BadRequest();
            _context.Entry(updatedPack).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var pack = await _context.CPacks.FindAsync(id);
            if (pack is null) return NotFound();
            _context.CPacks.Remove(pack);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
