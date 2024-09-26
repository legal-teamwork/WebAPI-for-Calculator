using Microsoft.AspNetCore.Mvc;
using ServiceCalculator.Models;
using System.Diagnostics;

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

        private string calculate(string expression)
        {
            string cwd = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory("..");
            System.IO.File.WriteAllText("input.txt", expression);
            Process.Start("Parser.exe").WaitForExit();
            string result = System.IO.File.ReadAllText("output.txt").Trim();
            Directory.SetCurrentDirectory(cwd);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Res>> PostExpAndResItem(Exp InputExp)
        {
            Res OutputResult = new Res();

            //

            OutputResult.Result = calculate(InputExp.Expression); // заменить на вычисление

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
