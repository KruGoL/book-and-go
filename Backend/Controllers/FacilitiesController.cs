using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FacilitiesController : ControllerBase
    {
        private readonly DataContext _context;
        public FacilitiesController(DataContext context) => _context = context;

        [HttpGet] public IActionResult GetFacilities()
        {
            return Ok(_context.FacilityList);
        }
        [AllowAnonymous]
        [HttpGet("{id}/bookings")] public IActionResult GetBookingsByFacisilityId(Guid id)
        {
            var facility = _context.FacilityList!.FirstOrDefault(x => x.Id == id);
            if(facility == null) return NotFound();
            var bookings = _context.BookingList!.Where(x => x.FacilityId == facility!.Id);
            if(bookings.Count() == 0) return NoContent(); 
            return Ok(bookings);
        }
        [AllowAnonymous]
        [HttpGet("{id}")] public ActionResult<Facility> GetFacility(Guid id)
        {
            var facility = _context.FacilityList!.FirstOrDefault(x => x.Id == id);
            if(facility == null) return NotFound();
            return facility;
        }

        [HttpPost] public ActionResult<Facility> PostFacility(Facility facility)
        {
            if(!FacilityExists(facility.Id)){
                _context.Add(facility);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetFacility), new { id = facility.Id }, facility);      
            }
            else return Conflict();
        }

        [HttpPut("{id}")] public IActionResult PutFacility(Guid id,Facility facility)
        {
            if (!FacilityExists(id)) return NotFound();
            _context.Update(facility);
            _context.SaveChanges();
            return NoContent();
        }

         [HttpDelete("{id}")] public ActionResult<Facility> DeleteFacility(Guid id)
         {
            var facility = _context.FacilityList!.FirstOrDefault(x => x.Id == id);
            if (facility == null) return NotFound();

            _context.FacilityList!.Remove(facility);
            _context.SaveChanges();
            return Ok(facility);
        }

        private bool FacilityExists(Guid id) => _context.FacilityList!.Any(b => b.Id == id);
    }
}