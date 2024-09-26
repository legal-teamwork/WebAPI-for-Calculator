using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCalculator.Models;

namespace ServiceCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpAndResController : ControllerBase
    {
        private readonly ExpAndResContext _context;

        public ExpAndResController(ExpAndResContext context)
        {
            _context = context;
        }

        // GET: api/ExpAndRes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpAndResItem>>> GetExpAndResItems()
        {
          if (_context.ExpAndResItems == null)
          {
              return NotFound();
          }
            return await _context.ExpAndResItems.Reverse().ToListAsync();
        }

        // GET: api/ExpAndRes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpAndResItem>> GetExpAndResItem(int id)
        {
          if (_context.ExpAndResItems == null)
          {
              return NotFound();
          }
            var expAndResItem = await _context.ExpAndResItems.FindAsync(id);

            if (expAndResItem == null)
            {
                return NotFound();
            }

            return expAndResItem;
        }

        // POST: api/ExpAndRes
        [HttpPost]
        public async Task<ActionResult<ExpAndResItem>> PostExpAndResItem(ExpAndResItem expAndResItem)
        {
          if (_context.ExpAndResItems == null)
          {
              return Problem("Entity set 'ExpAndResContext.ExpAndResItems'  is null.");
          }
            _context.ExpAndResItems.Add(expAndResItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExpAndResItem), new { id = expAndResItem.Id }, expAndResItem);
        }

        private bool ExpAndResItemExists(int id)
        {
            return (_context.ExpAndResItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
