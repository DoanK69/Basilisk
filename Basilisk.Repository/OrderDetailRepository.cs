using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class OrderDetailRepository : BaseRepository, IRepository<OrderDetail>
    {
        private static IRepository<OrderDetail> _instance = new OrderDetailRepository();
        public static IRepository<OrderDetail> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var orderDetail = _context.OrderDetails.SingleOrDefault(s => s.InvoiceNumber == (string)id);
                    _context.OrderDetails.Remove(orderDetail);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<OrderDetail> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.OrderDetails;
        }

        public OrderDetail GetSingle(object id)
        {
            var orderDetail = new OrderDetail();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                orderDetail = _context.OrderDetails.SingleOrDefault(s => s.Id == (long)id);
            }

            return orderDetail;
        }

        public bool Insert(OrderDetail model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.OrderDetails.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(OrderDetail model)
        {
            throw new NotImplementedException();
        }
    }
}
