using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.DTOs;
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
        public IActionResult AddCopy(CopyApiDto copy) {
            if (copy.CopyId != 0)
            {
                return Conflict();
            }

            for (int i = 0; i < copy.TotalCopies; i++) {
                _db.Dvdcopies.Add(new Dvdcopy { 
                    CopyId = 0,
                    DatePurchased = copy.DatePurchased,
                    DvdId = copy.DvdId,
                });
            }

            
            _db.SaveChanges();
            return Ok();
        }
    }
}
