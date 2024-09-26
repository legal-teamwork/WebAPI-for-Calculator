using Microsoft.AspNetCore.Mvc;
using ServiceCalculator.Models;

namespace ServiceCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculateExpressionController : ControllerBase
    {
        private readonly ExpAndResContext _context;

        public CalculateExpressionController(ExpAndResContext context)
        {
            _context = context;
        }

        public class Exp
        {
            public String? Expression { get; set; }
        }

        public class Res
        {
            public String? Result { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<Res>> PostExpAndResItem(Exp InputExp)
        {
            Res OutputResult = new Res();

            //

            OutputResult.Result = InputExp.Expression; // заменить на вычисление

            //

            ExpAndResItem expAndResItem = new ExpAndResItem
            {
                Result = OutputResult.Result,
                Expression = InputExp.Expression
            };

            if (_context.ExpAndResItems == null)
            {
                return Problem("Entity set 'ExpAndResContext.ExpAndResItems'  is null.");
            }
            _context.ExpAndResItems.Add(expAndResItem);
            await _context.SaveChangesAsync();
            return OutputResult;
        }
    }
}
