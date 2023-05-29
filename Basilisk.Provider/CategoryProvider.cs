using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{

    public class CategoryProvider : BaseProvider
    {
        private static CategoryProvider _intance = new CategoryProvider();

        public static CategoryProvider GetProvider()
        {
            return _intance;
        }

        public IEnumerable<GridCategoryViewModel> GetDataIndex(string searchName)
        {
            using (var _context = new BasiliskTFContext())
            {
                var categories = (from cat in _context.Categories
                                  where cat.Name.Contains(searchName)
                                  select new GridCategoryViewModel
                                  {
                                      ID = cat.Id,
                                      Name = cat.Name,
                                      Description = cat.Description
                                  }).ToList();

                return categories;
            };
        }
        public IndexCategoryViewModel GetIndex(int page, string searchName)
        {
            searchName = string.IsNullOrEmpty(searchName) ? "" : searchName;

            IEnumerable<GridCategoryViewModel> data = GetDataIndex(searchName);
            int totalCategory = data.Count();
            int totalData = data.Count();
            data = data.Skip(GetSkip(page)).Take(TotalDataPerPage);

            var model = new IndexCategoryViewModel
            {
                SearchName = searchName,
                Grid = data,
                TotalData = totalData,
                TotalHalaman = GetHalaman(totalData),
                TotalCategory = totalCategory
            };

            return model;
        }

        public IEnumerable<GridProductViewModel> GetDetail(long id)
        {
            using (var _context = new BasiliskTFContext())
            {
                //var product = ((CategoryRepository)CategoryRepository.GetRepository()).DetailProduct(id);
                var product = ProductRepository.GetRepository().GetAll();
                var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
                var query = (from prod in product
                             where prod.CategoryId == id
                             select new GridProductViewModel
                             {
                                 Id = prod.Id,
                                 ProductName = prod.Name,
                                 CategoryName = prod.Category.Name,
                                 Description = prod.Description,
                                 Price = prod.Price.ToString("C", indoFormat),
                                 Stock = prod.Stock,
                                 Discontinue = prod.Discontinue ? "Discontinue" : "Continue"
                             }).ToList();

                return query;
            }

        }

        public GridCategoryViewModel GetDetailCategory(long id)
        {
            //var product = ((CategoryRepository)CategoryRepository.GetRepository()).DetailProduct(id);
            var category = CategoryRepository.GetRepository().GetAll();
            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var query = (from cat in category
                         where cat.Id == id
                         select new GridCategoryViewModel
                         {
                             ID = cat.Id,
                             Name = cat.Name,
                             Description = cat.Description
                         }).SingleOrDefault();

            return query;
        }

        public void PostAdd(CreateUpdateViewModel model)
        {
            try
            {
                var category = new Category();
                MapingModel<Category, CreateUpdateViewModel>(category, model);

                CategoryRepository.GetRepository().Insert(category);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CreateUpdateViewModel GetEditPopup(long id)
        {
            var model = new CreateUpdateViewModel();
            try
            {
                var oldCategory = CategoryRepository.GetRepository().GetSingle(id);
                MapingModel(model, oldCategory);
            }
            catch (Exception)
            {

                throw;
            }
            return model;
        }

        public JsonResultViewModel GetUpdateApi(long id)
        {
            var result = new JsonResultViewModel();
            var model = new CreateUpdateViewModel();
            try
            {
                var oldCategory = CategoryRepository.GetRepository().GetSingle(id);
                if (oldCategory != null)
                {
                    MapingModel<CreateUpdateViewModel, Category>(model, oldCategory);
                    result.Code = 200;
                    result.Message = "Data Found";
                    result.Success = true;
                    result.ReturnObject = model;
                }
                result.Code = 404;
                result.Message = "Data Not Found";
                result.Success = false;
                result.ReturnObject = new CreateUpdateViewModel();
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public CreateUpdateViewModel GetUpdate(long id)
        {
            var model = new CreateUpdateViewModel();
            try
            {
                var oldCategory = CategoryRepository.GetRepository().GetSingle(id);
                if (oldCategory != null)
                {
                    MapingModel<CreateUpdateViewModel, Category>(model, oldCategory);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        public void PostUpdate(CreateUpdateViewModel model)
        {
            try
            {
                var oldCategory = CategoryRepository.GetRepository().GetSingle(model.Id);
                MapingModel<Category, CreateUpdateViewModel>(oldCategory, model);
                CategoryRepository.GetRepository().Update(oldCategory);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PostUpdate(long id, string description)
        {
            try
            {
                CategoryRepository.GetRepository().UpdateDescription(id, description);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool GetDelete(long id)
        {

            bool anyDependentProduct = ProductRepository.GetRepository().GetAll().Any(product => product.CategoryId == id);

            return anyDependentProduct;

        }

        public void PostDelete(long id)
        {

            try
            {
                var entityModel = CategoryRepository.GetRepository().GetSingle(id);
                CategoryRepository.GetRepository().Delete(id);
            }
            catch
            {
                throw;
            }

        }

        public int GetCount(long id)
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                var product = ProductRepository.GetRepository().GetAll();
                return product.Where(p => p.CategoryId == id).Count();
            }
        }
    }
}
