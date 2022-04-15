using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Api
{
    [ApiController]
    [Route("api/copies")]
    public class CopyApi:ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CopyApi(ApplicationDbContext database)
        {
            _db = database;
        }

        [HttpPost]
        public IActionResult AddCopy(Dvdcopy copy) {
            if (copy.CopyId != 0)
            {
                return Conflict();
            }
            _db.Dvdcopies.Add(copy);
            _db.SaveChanges();
            return Ok(copy);
        }
    }
}
