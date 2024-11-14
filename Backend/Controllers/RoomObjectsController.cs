using Backend.Model;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/rooms/{roomId}/objects")]
    public class RoomObjectsController : ControllerBase
    {
        private readonly DataContext _context;
        public RoomObjectsController(DataContext context){
            _context = context;
        }
        [HttpGet]
        public ActionResult GetRoomObjects(Guid roomId) //Get objects in one room
        {
            var room = _context.RoomList?.FirstOrDefault(x => x.Id == roomId);
            if (room == null){
                return NotFound();
            }

            return Ok(_context.BookableObjectList?.Where(x => x.RoomId == roomId));
        }
    }
}