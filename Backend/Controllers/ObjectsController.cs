using Microsoft.AspNetCore.Mvc;
using Backend.Model;

namespace Backend.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ObjectsController : ControllerBase
    {
        private readonly DataContext _context;
        public ObjectsController(DataContext context) => _context = context;
        
        [HttpGet] public IActionResult GetObjects()
        {
            return Ok(_context.BookableObjectList);
        }

        [HttpGet("{id}")] public ActionResult<BookableObject> GetBookableObject(string id)
        {
            var bookableObject = _context.BookableObjectList!.FirstOrDefault(x => x.Id == id);
            if(bookableObject == null) return NotFound();
            return bookableObject;
        }

        [HttpPost] public ActionResult<BookableObject> PostBookableObject(BookableObject bookableObject )
        {   
            if(_context.RoomList!.Find(bookableObject.RoomId) == null) return BadRequest(); //Cant add an object without room

            if(bookableObject.X < 0 || bookableObject.Y < 0) return BadRequest(); //Cant add an object with negative coordinates

            if(bookableObject.Height < 0 || bookableObject.Width < 0) return BadRequest(); //Cant add an object with negative height or width

            //if(ObjectsCoordinatesExist(bookableObject)) return Conflict(); //Cant add an object if its coordinates already exist

            if(!BookableObjectExists(bookableObject.Id)){
                _context.Add(bookableObject);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetBookableObject), new { id = bookableObject.Id }, bookableObject);      
            }
            else return Conflict();
        }

        [HttpPut("{id}")] public IActionResult PutBookableObject(string id, BookableObject bookableObject)
        {
            if(_context.RoomList!.Find(bookableObject.RoomId) == null) return BadRequest(); //Cant edit an object without room

            if (!BookableObjectExists(id)) return NotFound();
            _context.Update(bookableObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")] public ActionResult<BookableObject> DeleteBookableObject(string id)
        {
            var bookableObject = _context.BookableObjectList!.FirstOrDefault(x => x.Id == id);
            if (bookableObject == null) return NotFound();

            _context.BookableObjectList!.Remove(bookableObject);
            _context.SaveChanges();
            return Ok(bookableObject);
        }

        private bool BookableObjectExists(string id) => _context.BookableObjectList!.Any(b => b.Id == id);
        private bool ObjectsCoordinatesExist(BookableObject bookableObject)
        {
            var coordinateX = _context.BookableObjectList!.Any(x => x.X == bookableObject.X);
            var coordinateY = _context.BookableObjectList!.Any(x => x.Y == bookableObject.Y);
            if(coordinateX && coordinateY) return true;
            else return false;
        }

        //private bool BookableObjectExists(string id) => _context.BookableObjectList!.Any(b => b.Id == id);
    }
}