using Microsoft.AspNetCore.Mvc;
using MyShop.Data;
using MyShop.Models;

namespace MyShop.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ActorApiController(ApplicationDbContext database)
        {
            _db = database;
        }

        [HttpPost]
        public async Task<IActionResult> AddActor(Actor actor)
        {
            if (actor.ActorId == 0)
            {
                await _db.Actors.AddAsync(actor);
                return Ok();
            }

            return Conflict();

        }

        [HttpGet]
        public List<Actor> GetAllActor() {
            return _db.Actors.ToList();
        }

    }
}
