using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Class;
using Web_Api.Data;
using Web_Api.DBModel;


namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ContactsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhoneBook>>> GetPhoneBooks()
        {

            return await _context.PhoneBooks
                        .Where(p => !p.Deleted)
                        .ToListAsync();

        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneBook>> GetPhoneBook(int id)
        {
            var phoneBook = await _context.PhoneBooks.FindAsync(id);


            if (phoneBook == null || phoneBook.Deleted)
            {
                return NotFound(".کاربر مورد نظر یافت نشد");
            }

            return Ok(phoneBook);
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> DtoUpdate(int id, [FromBody] DtoUpdate dtoUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var phoneBook = await _context.PhoneBooks.FindAsync(id);
            if (phoneBook == null)
            {
                return NotFound();
            }

            // به‌روزرسانی مقادیر محصول
            phoneBook.FirstName = dtoUpdate.FirstName;
            phoneBook.LastName = dtoUpdate.LastName;
            phoneBook.PhoneNumber = dtoUpdate.PhoneNumber;


            _context.PhoneBooks.Update(phoneBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhoneBook>> PostPhoneBook(DtoUpdate dtoUpdate)
        {
            var PhoneBook = new PhoneBook
            {
                FirstName = dtoUpdate.FirstName,
                LastName = dtoUpdate.LastName,
                PhoneNumber = dtoUpdate.PhoneNumber,
                Deleted = false

            };

            _context.PhoneBooks.Add(PhoneBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhoneBook", new { id = PhoneBook.ID }, PhoneBook);
        } 

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoneBook(int id)
        {
            var phoneBook = await _context.PhoneBooks.FindAsync(id);
            if (phoneBook == null || phoneBook.Deleted)
            {
                return NotFound();
            }
			phoneBook.Deleted = true;

			_context.PhoneBooks.Update(phoneBook);
            await _context.SaveChangesAsync();

            var maxId = await _context.PhoneBooks.MaxAsync(c => (int?)c.ID) ?? 0;
            await _context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('PhoneBooks', RESEED, {maxId})");


            return NoContent();
        }

        private bool PhoneBookExists(int id)
        {
            return _context.PhoneBooks.Any(e => e.ID == id);
        }
	}
}
