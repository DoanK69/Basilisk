using Basilisk.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Repository
{
    public class CategoryRepository : BaseRepository, IRepository<Category>
    {
        //private static IRepository<Category> _instance = new CategoryRepository();

        //public static IRepository<Category> GetRepository() 
        //{
        //    return _instance; 
        //}
        private static CategoryRepository _instance = new CategoryRepository();

        public static CategoryRepository GetRepository() 
        {
            return _instance; 
        }

        public bool Delete(object id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCategory = _context.Categories.SingleOrDefault(c => c.Id == (long)id);
                    _context.Categories.Remove(oldCategory);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }
        }

        public IQueryable<Category> GetAll()
        {
            var _context = new BasiliskTFContext();
            return _context.Categories;
        }

        public Category GetSingle(object id)
        {
            var category = new Category();
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                category = _context.Categories.SingleOrDefault(c => c.Id == (long)id);
            }

            return category;
        }

        public bool Insert(Category model)
        {
            try
            {
                using (BasiliskTFContext _context = new BasiliskTFContext())
                {
                    _context.Categories.Add(model);
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Category model)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                try
                {
                    var oldCategory = _context.Categories.SingleOrDefault(c => c.Id == model.Id);
                    MapingModel(oldCategory, model);

                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void UpdateDescription(long id, string description)
        {
            using (var _context = new BasiliskTFContext())
            {
                try
                {
                    var category = _context.Categories.SingleOrDefault(c => c.Id == id);
                    category.Description = description;
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            };
        }

        
    }
}
