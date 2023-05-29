using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class CustomerRepository : BaseRepository, IRepository<Customer>
    {
        private static IRepository<Customer> _instance = new CustomerRepository();
        public static IRepository<Customer> GetRepository()
        {
            return _instance;
        }

        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCustomer = _context.Customers.SingleOrDefault(s => s.Id == (long)id);
                    _context.Customers.Remove(oldCustomer);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public IQueryable<Customer> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Customers;
        }

        public Customer GetSingle(object id)
        {
            var customer = new Customer();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                customer = _context.Customers.SingleOrDefault(s => s.Id == (long)id);
            }

            return customer;
        }

        public bool Insert(Customer model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Customers.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Customer model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldSalesman = _context.Customers.SingleOrDefault(s => s.Id == model.Id);
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
