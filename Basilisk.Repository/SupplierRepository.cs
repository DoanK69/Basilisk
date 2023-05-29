using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class SupplierRepository : BaseRepository, IRepository<Supplier>
    {
        private static IRepository<Supplier> _instance = new SupplierRepository();
        public static IRepository<Supplier> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldSupplier = _context.Suppliers.SingleOrDefault(c => c.Id == (long)id);
                    _context.Suppliers.Remove(oldSupplier);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Supplier> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Suppliers;
        }

        public Supplier GetSingle(object id)
        {
            var supplier = new Supplier();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                supplier = _context.Suppliers.SingleOrDefault(s => s.Id == (long)id);
            }

            return supplier;
        }

        public bool Insert(Supplier model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Suppliers.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Supplier model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldSupplier = _context.Suppliers.SingleOrDefault(c => c.Id == model.Id);
                    MapingModel(oldSupplier, model);

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
