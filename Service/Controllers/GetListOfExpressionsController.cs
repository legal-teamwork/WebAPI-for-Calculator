using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceCalculator.Models;

namespace ServiceCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetListOfExpressionsController : ControllerBase
    {
        private readonly ExpAndResContext _context;

        public GetListOfExpressionsController(ExpAndResContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpAndResItem>>> GetExpAndResItems()
        {
            if (_context.ExpAndResItems == null)
            {
                return NotFound();
            }
            return await _context.ExpAndResItems.Reverse().ToListAsync();
        }
    }
}
