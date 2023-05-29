using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class SalesmenRepository : BaseRepository, IRepository<Salesman>
    {
        private static IRepository<Salesman> _instance = new SalesmenRepository();
        public static IRepository<Salesman> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldSalesmen = _context.Salesmen.SingleOrDefault(s => s.EmployeeNumber == (string)id);
                    _context.Salesmen.Remove(oldSalesmen);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Salesman> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Salesmen;
        }

        public Salesman GetSingle(object id)
        {
            var salesmen = new Salesman();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                salesmen = _context.Salesmen.SingleOrDefault(s => s.EmployeeNumber == (string)id);
            }

            return salesmen;
        }

        public bool Insert(Salesman model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Salesmen.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Salesman model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldSalesman = _context.Salesmen.SingleOrDefault(s => s.EmployeeNumber == model.EmployeeNumber);
                    MapingModel(oldSalesman, model);

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
}
