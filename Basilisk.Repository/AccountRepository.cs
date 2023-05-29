using Basilisk.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class AccountRepository
    {
        public static string GetRole(string username)
        {
            using (var _context = new BasiliskTFContext())
            {
                return _context.Accounts.SingleOrDefault(a => a.Username == username).Role;
            }
        }

        public static bool GetAuthentication(string username, string password)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                var accountByUsername = _context.Accounts.SingleOrDefault(a => a.Username == username && a.Password == password);
                if (accountByUsername != null)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCategory = _context.Accounts.SingleOrDefault(c => c.Username == (string)id);
                    _context.Accounts.Remove(oldCategory);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }
        }

        public static IQueryable<Account> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Accounts;
        }

        public static Account GetSingle(object id)
        {
            var account = new Account();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                account = _context.Accounts.SingleOrDefault(c => c.Username == (string)id);
            }

            return account;
        }

        public static bool Insert(Account model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Accounts.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Update(Account model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldAccount = _context.Accounts.SingleOrDefault(c => c.Username == model.Username);
                    oldAccount.Username = model.Username;
                    oldAccount.Password = model.Password;
                    oldAccount.Role = model.Role;

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
