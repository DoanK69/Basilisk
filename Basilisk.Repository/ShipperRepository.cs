using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class ShipperRepository : BaseRepository, IRepository<Delivery>
    {
        private static IRepository<Delivery> _instance = new ShipperRepository();
        public static IRepository<Delivery> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldDelivery = _context.Deliveries.SingleOrDefault(s => s.Id == (long)id);
                    _context.Deliveries.Remove(oldDelivery);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Delivery> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Deliveries;
        }

        public Delivery GetSingle(object id)
        {
            var deliveries = new Delivery();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                deliveries = _context.Deliveries.SingleOrDefault(s => s.Id == (long)id);
            }

            return deliveries;
        }

        public bool Insert(Delivery model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Deliveries.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Delivery model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldDelivery = _context.Deliveries.SingleOrDefault(s => s.Id == model.Id);
                    MapingModel(oldDelivery, model);

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
