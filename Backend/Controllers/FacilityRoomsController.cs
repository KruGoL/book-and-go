using Microsoft.AspNetCore.Mvc;
using Backend.Model;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/facilities/{facilityId}/rooms")]
    public class FacilityRoomsController : ControllerBase
    {
        private readonly DataContext _context;
        public FacilityRoomsController(DataContext context){
            _context = context;
        }
        [HttpGet]
        public ActionResult GetFacilityRooms(Guid facilityId) //Get rooms in one facility
        {
           var facility = _context.FacilityList!.FirstOrDefault(x => x.Id == facilityId);
            if(facility == null) return NotFound();
            var rooms = _context.RoomList!.Where(x => x.FacilityId == facility!.Id);
            if(rooms.Count() == 0) return NoContent(); 
            return Ok(rooms);
        }
    }
}