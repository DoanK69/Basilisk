using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class CartRepository : BaseRepository, IRepository<Cart>
    {
        private static IRepository<Cart> _instance = new CartRepository();
        public static IRepository<Cart> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCustomer = _context.Carts.SingleOrDefault(s => s.Id == (int)id);
                    _context.Carts.Remove(oldCustomer);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Cart> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Carts;
        }

        public Cart GetSingle(object id)
        {
            var cart = new Cart();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                cart = _context.Carts.SingleOrDefault(s => s.Id == (int)id);
            }

            return cart;
        }

        public bool Insert(Cart model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Carts.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Cart model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldOrder = _context.Carts.SingleOrDefault(o => o.Id == model.Id);
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
