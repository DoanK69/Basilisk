using Basilisk.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class RegionRepository : IRepository<Region>
    {
        private static IRepository<Region> _instance = new RegionRepository();

        public static IRepository<Region> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            try
            {
                var _context = new BasiliskTFContext();
                var oldRegion = _context.Regions.SingleOrDefault(r => r.Id == (long)id);
                _context.Regions.Remove(oldRegion);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<Region> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Regions;
        }

        public Region GetSingle(object id)
        {
            var region = new Region();
            BasiliskTFContext _context = new BasiliskTFContext();
            region = _context.Regions.SingleOrDefault(r => r.Id == (long)id);
            return region;
        }

        public bool Insert(Region model)
        {
            try
            {
                var _context = new BasiliskTFContext();
                _context.Regions.Add(model);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Region model)
        {
            var _context = new BasiliskTFContext();
            try
            {
                var oldRegion = _context.Regions.SingleOrDefault(r => r.Id == model.Id);
                oldRegion.City = model.City;
                oldRegion.Remark = model.Remark;
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
