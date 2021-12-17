using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StarChart.Models;
using System;

namespace StarChart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CelestialObject> CelestialObjects { get; set; }

        public CelestialObject GetFullObjectById(int id)
        {
            var celestialObject = CelestialObjects.Where(c => c.Id == id).FirstOrDefault();
            if (celestialObject == null)
            {
                return null;
            }
            FillSatelite(celestialObject);

            return celestialObject;
        }

        private void FillSatelite(CelestialObject obj)
        {
            obj.Satellites = CelestialObjects.Where(c => c.OrbitedObjectId == obj.Id).ToList();
        }

        public IList<CelestialObject> GetByName(string name)
        {
            var celestialObjects = CelestialObjects.Where(c => c.Name == name).ToList();
            if (celestialObjects?.Any() == false)
            {
                return null;
            }
            foreach (var obj in celestialObjects)
            {
                FillSatelite(obj);
            }

            return celestialObjects;
        }

        public IList<CelestialObject> GetAll()
        {
            var celestialObjects = CelestialObjects.ToList();
            foreach (var obj in celestialObjects)
            {
                FillSatelite(obj);
            }

            return celestialObjects;
        }
    }
}
