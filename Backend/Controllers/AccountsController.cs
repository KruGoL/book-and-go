using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Model;
using Backend.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly DataContext _context;

        public AccountsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet] //Get authorized account
        public IActionResult Get()
        {
            var account = _context.AccountList!.AsQueryable().FirstOrDefault(x => x.Id == UserData.GetUserId(this.HttpContext));
            return Ok(account);
        }
        [HttpGet("facilities")] public IActionResult GetUserFacilities()
        {
            var facility = _context.FacilityList!.Where(x => x.OwnerId == UserData.GetUserId(this.HttpContext));
            if(facility.Count() == 0) return NotFound();
            return Ok(facility);
        }
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(Guid id)
        {
            var account = _context.AccountList!.Find(id);
            if (account == null) return NotFound();
            return account;
        }

        [HttpPut("{id}")]
        public IActionResult PutAccount(Guid id, Account account)
        {
            var oldAccount = _context.AccountList!.AsNoTracking().FirstOrDefault(x => x.Id == account.Id && x.Id == UserData.GetUserId(this.HttpContext));
            if (id != account.Id || oldAccount == null)
            {
                return NotFound();
            }
            _context.Update(account);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<Account> PostAccount(Account account)
        {
            if (!AccountExists(account.Id))
            {
                _context.Add(account);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
            }
            else return Conflict();
        }

        [HttpDelete("{id}")]
        public ActionResult<Account> DeleteAccount(Guid id)
        {
            var account = _context.AccountList!.Find(id);
            if (account == null) return NotFound();

            _context.AccountList!.Remove(account);
            _context.SaveChanges();
            return Ok();
        }

        private bool AccountExists(Guid id) => _context.AccountList!.Any(a => a.Id == id);

    }
}