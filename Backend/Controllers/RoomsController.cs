using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Backend.Model;

namespace Backend.Controllers
{

   // [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly DataContext _context;
        public RoomsController(DataContext context) => _context = context;
        
        [HttpGet] public IActionResult GetRooms()
        {
            return Ok(_context.RoomList);
        }

        [HttpGet("{id}")] public ActionResult<Room> GetRoom(Guid id)
        {
            var room = _context.RoomList!.FirstOrDefault(x => x.Id == id);
            if(room == null) return NotFound();
            return room;
        }

        [HttpPut("{id}")] public IActionResult PutRoom(Guid id, Room room)
        {
            if(_context.FacilityList!.Find(room.FacilityId) == null) return BadRequest(); //Cant edit a room without facility

            if (!RoomExists(id)) return NotFound();
            _context.Update(room);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost] public ActionResult<Room> PostRoom(Room room)
        {
            if(_context.FacilityList!.Find(room.FacilityId) == null) return BadRequest(); //Cant add a room without facility

            if(!RoomExists(room.Id)){
                _context.Add(room);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetRooms), new { id = room.Id }, room);      
            }
            else return Conflict();
        }

        [HttpDelete("{id}")] public ActionResult<Room> DeleteRoom(Guid id)
        {
            var room = _context.RoomList!.FirstOrDefault(x => x.Id == id);
            if (room == null) return NotFound();

            _context.RoomList!.Remove(room);
            _context.SaveChanges();
            return Ok(room);
        }

        private bool RoomExists(Guid id) => _context.RoomList!.Any(r => r.Id == id);
    }
}