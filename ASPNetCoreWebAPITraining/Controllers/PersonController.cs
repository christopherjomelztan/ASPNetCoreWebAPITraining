using Domain.Common;
using Domain.Entities;
using Infrastructure.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNetCoreWebAPITraining.Controllers
{
    [ApiController]
    [Route("api/Person")]
    public class PersonController : ControllerBase
    {
        private readonly IDbContextFactory _dbContextFactory;

        public PersonController(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // GET: api/Person
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            using var context = _dbContextFactory.CreateDbContext();
            return await context.Persons.ToListAsync();
        }

        // GET: api/Person/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var person = await context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Person/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            using var context = _dbContextFactory.CreateDbContext();
            if (id != person.Id) return BadRequest();
            
            context.Entry(person).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // POST: api/Person
        [HttpPost]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Persons.Add(person);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.Id }, person);
        }

        // DELETE: api/Person/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            var person = await context.Persons.FindAsync(id);
            
            if (person == null) return NotFound();
            
            context.Persons.Remove(person);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool PersonExists(int id)
        {
            using var context = _dbContextFactory.CreateDbContext();
            return context.Persons.Any(e => e.Id == id);
        }
    }
}
