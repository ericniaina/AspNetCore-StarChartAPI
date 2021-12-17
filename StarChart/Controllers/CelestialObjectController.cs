using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.GetFullObjectById(id);
            if (celestialObject == null)
            {
                return NotFound();
            }

            return Ok(celestialObject);
        }

        [HttpGet("{name}", Name = "GetByName")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.GetByName(name);
            if (celestialObjects == null)
            {
                return NotFound();
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.GetAll();
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById",
                new { Id = celestialObject.Id },
                celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CelestialObject celestialObject)
        {
            var celestialObjectToUpdate = _context.GetFullObjectById(id);
            if (celestialObjectToUpdate == null)
            {
                return NotFound();
            }

            celestialObjectToUpdate.Name = celestialObject.Name;
            celestialObjectToUpdate.OrbitalPeriod = celestialObject.OrbitalPeriod;
            celestialObjectToUpdate.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.CelestialObjects.Update(celestialObjectToUpdate);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObjectToUpdate = _context.GetFullObjectById(id);
            if (celestialObjectToUpdate == null)
            {
                return NotFound();
            }

            celestialObjectToUpdate.Name = name;

            _context.CelestialObjects.Update(celestialObjectToUpdate);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleteCandidates = _context.CelestialObjects.Where(c => c.Id == id || c.OrbitedObjectId == id).ToList();
            if(deleteCandidates?.Any() == false)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(deleteCandidates);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
