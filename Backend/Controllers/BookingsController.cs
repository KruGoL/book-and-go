using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.AspNetCore.Authorization;
using Backend.Helper;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase{
        private readonly DataContext _context;
        public BookingsController(DataContext context) => _context = context;
        
        [AllowAnonymous][HttpGet] public IActionResult GetBookings(DateTime? start, DateTime? end) //Filter by BookingTime
        {
            var query = _context.BookingList!.AsQueryable();

            if (start != null)
            {
                query = query.Where(x => x.BookingDate != null && x.BookingDate >= start);
            }

            if (end != null)
            {
                query = query.Where(x => x.BookingDate != null && x.BookingDate <= end);
            }

            return Ok(_context.BookingList);
        }
        [AllowAnonymous]
        [HttpGet("{id}")] public ActionResult<Booking> GetBooking(Guid id)
        {
            var booking = _context.BookingList!.FirstOrDefault(x => x.Id == id);
            if(booking == null) return NotFound();
            return booking;
        }
        [AllowAnonymous][HttpPost] public ActionResult<Booking> PostBooking(Booking booking)
        {   
            //if(_context.BookableObjectList!.Find(booking.ObjectId) == null) return BadRequest(); //Cant add a booking without object

            if(booking.Duration == null)
            {
                //booking.Duration = booking.BookingTime.AddHours(3); //If Duration field is empty, basic Duration = BookingTime + 3 hours
            }

            if(BookingExpired(booking)) return BadRequest();
            if(!BookingExists(booking.Id))
            {
                _context.Add(booking);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);      
            }
            else return Conflict();
        }

        [HttpPut("{id}")] public ActionResult<Booking> PutBooking(Guid id,Booking booking)
        {
            var oldBooking = _context.BookingList!.AsNoTracking().FirstOrDefault(x => x.Id == booking.Id);
            if (id !=  booking.Id ||  oldBooking == null)
            {
                return NotFound();
            }
            //if(_context.BookableObjectList!.Find(booking.ObjectId) == null) return BadRequest(); //Cant edit a booking without object
            
            if(booking.Duration == null)
            {
                //booking.Duration = booking.BookingTime.AddHours(3); //If Duration field is empty, basic Duration = BookingTime + 3 hours
            }

            if (BookingExpired(booking)) return BadRequest();
            _context.Update(booking);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
        }

        [HttpDelete("{id}")] public ActionResult<Booking> DeleteBooking(Guid id)
        {
            var booking = _context.BookingList!.FirstOrDefault(x => x.Id == id);
            if (booking == null) return NotFound();

            _context.BookingList!.Remove(booking);
            _context.SaveChanges();
            return Ok(booking);
        }

        private bool BookingExists(Guid id) => _context.BookingList!.Any(b => b.Id == id);
        private bool BookingExists(Booking booking) => _context.BookingList!.Any(b => b.Id == booking.Id);
        private bool BookingExpired(Booking booking) => booking.BookingDate < DateTime.Now;
    }
}