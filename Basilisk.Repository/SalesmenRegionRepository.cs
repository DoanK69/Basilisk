using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class SalesmenRegionRepository : BaseRepository, IRepository<SalesmenRegion>
    {
        private static IRepository<SalesmenRegion> _instance = new SalesmenRegionRepository();
        public static IRepository<SalesmenRegion> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SalesmenRegion> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.SalesmenRegions;
        }

        public SalesmenRegion GetSingle(object id)
        {
            throw new NotImplementedException();        
        }

        public bool Insert(SalesmenRegion model)
        {
            throw new NotImplementedException();
        }

        public bool Update(SalesmenRegion model)
        {
            throw new NotImplementedException();
        }
    }
}
