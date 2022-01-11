using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CountryApi.Models;
using CountryApi.Attributes;

namespace CountryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class CountryItemsController : ControllerBase
    {
        private readonly CountryContext _context;

        public CountryItemsController(CountryContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Produces("application/json")]
        [SwaggerOperation("Zwraca wszystkie kraje.", "Używa EF")]
        [SwaggerResponse(200, "Sukces", Type = typeof(List<CountryItem>))]        
        public async Task<ActionResult<IEnumerable<CountryItem>>> GetCountryItems()
        {
            return await _context.CountryItems.ToListAsync(); 
        }

        [HttpGet("{id}")]        
        [Produces("application/json")]
        [SwaggerOperation("Zwraca kraj o podanym {id}.", "Używa EF FindAsync()")]
        [SwaggerResponse(200, "Znaleziono kraj o podanym {id}", Type = typeof(CountryItem))]
        [SwaggerResponse(404, "Nie znaleziono kraju o podanym {id}")]
        public async Task<ActionResult<CountryItem>> GetCountryItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz odczytać", Required = true)]
            int id)
        {
            var countryItem = await _context.CountryItems.FindAsync(id);

            if (countryItem == null)
            {
                return NotFound(); 
            }

            return countryItem;   
        }
        
        [HttpPut("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Aktualizuje kraj o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Zaktualizowano kraj o podanym {id}")]
        [SwaggerResponse(400, "Nie rozpoznano danych wejściowych")]
        [SwaggerResponse(404, "Nie znaleziono kraju o podanym {id}")]
        public async Task<IActionResult> PutCountryItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz zaktualizować", Required = true)]
            int id,
            [SwaggerParameter("Definicja kraju", Required = true)]
            CountryItem countryItem)
        {
            if (id != countryItem.Id)
            {
                return BadRequest(); //http 400
            }

            _context.Entry(countryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryItemExists(id))
                {
                    return NotFound();  //http 404
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); //http 204
        }

    
        [HttpPost]
        [Produces("application/json")]
        [SwaggerOperation("Tworzy nowy kraj.", "Używa EF")]
        [SwaggerResponse(201, "Zapis z sukcesem.", Type = typeof(CountryItem))]
        public async Task<ActionResult<CountryItem>> PostCountryItem(
            [SwaggerParameter("Definicja kraju", Required = true)]
            CountryItem countryItem)
        {
            _context.CountryItems.Add(countryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountryItem", new { id = countryItem.Id }, countryItem);  
        }


        [HttpDelete("{id}")]
        [Produces("application/json")]
        [SwaggerOperation("Usuwa kraj o podanym {id}.", "Używa EF")]
        [SwaggerResponse(204, "Usunięto kraj o podanym {id}")]        
        [SwaggerResponse(404, "Nie znaleziono kraju o podanym {id}")]
        public async Task<IActionResult> DeleteCountryItem(
            [SwaggerParameter("Podaj nr zadnia które chcesz usunąć", Required = true)]
            int id)
        {
            var countryItem = await _context.CountryItems.FindAsync(id);
            if (countryItem == null)
            {
                return NotFound(); 
            }

            _context.CountryItems.Remove(countryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool CountryItemExists(int id)
        {
            return _context.CountryItems.Any(e => e.Id == id);
        }
    }
}
