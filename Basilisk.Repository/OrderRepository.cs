using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class OrderRepository : BaseRepository, IRepository<Order>
    {
        private static IRepository<Order> _instance = new OrderRepository();
        public static IRepository<Order> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCustomer = _context.Orders.SingleOrDefault(s => s.InvoiceNumber == (string)id);
                    _context.Orders.Remove(oldCustomer);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Order> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Orders;
        }

        public Order GetSingle(object id)
        {
            var order = new Order();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                order = _context.Orders.SingleOrDefault(s => s.InvoiceNumber == (string)id);
            }

            return order;
        }

        public bool Insert(Order model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Orders.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Order model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldOrder = _context.Orders.SingleOrDefault(o => o.InvoiceNumber == model.InvoiceNumber);
                    MapingModel(oldOrder, model);

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
