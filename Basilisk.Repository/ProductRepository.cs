using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class ProductRepository : BaseRepository, IRepository<Product>
    {
        private static IRepository<Product> _instance = new ProductRepository();

        public static IRepository<Product> GetRepository()
        {
            return _instance;
        }
        public bool Delete(object id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Products;
        }

        public Product GetSingle(object id)
        {
            var product = new Product();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                product = _context.Products.SingleOrDefault(s => s.Id == (long)id);
            }

            return product;
        }

        public bool Insert(Product model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Products.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Product model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldProduct = _context.Products.SingleOrDefault(s => s.Id == model.Id);
                    MapingModel(oldProduct, model);

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
