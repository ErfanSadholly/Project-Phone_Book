using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_Api.Class;
using Web_Api.Data;


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
            return await _context.PhoneBooks.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhoneBook>> GetPhoneBook(int id)
        {
            var phoneBook = await _context.PhoneBooks.FindAsync(id);

            if (phoneBook == null)
            {
                return NotFound();
            }

            return phoneBook;
        }

        // PUT: api/Contacts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneBook(int id, PhoneBook phoneBook)
        {
            if (id != phoneBook.ID)
            {
                return BadRequest();
            }

            _context.Entry(phoneBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contacts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhoneBook>> PostPhoneBook(PhoneBook phoneBook)
        {
            _context.PhoneBooks.Add(phoneBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPhoneBook", new { id = phoneBook.ID }, phoneBook);
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoneBook(int id)
        {
            var phoneBook = await _context.PhoneBooks.FindAsync(id);
            if (phoneBook == null)
            {
                return NotFound();
            }

            _context.PhoneBooks.Remove(phoneBook);
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
