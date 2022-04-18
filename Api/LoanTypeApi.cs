using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Api
{
    [ApiController]
    [Route("api/loantype")]
    public class LoanTypeApi : ControllerBase
    {
        public ApplicationDbContext _db { get; set; }

        public LoanTypeApi(ApplicationDbContext database)
        {
            _db = database;
        }

        [HttpPost]
        public async Task<IActionResult> AddLoanType(LoanType loanType)
        {
            if (0 != loanType.LoanTypeId)
            {
                return Conflict();
            }

            _db.LoanTypes.Add(loanType);
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
