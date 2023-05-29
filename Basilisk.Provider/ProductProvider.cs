using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Orders;
using Basilisk.ViewModel.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class ProductProvider : BaseProvider
    {
        private static ProductProvider _instance = new ProductProvider();

        public static ProductProvider GetProvider()
        {
            return _instance;
        }

        private IEnumerable<GridProductViewModel> GetDataIndex()
        {
            var product = ProductRepository.GetRepository().GetAll();

            var products = (from prod in product
                            orderby prod.Id
                            select new GridProductViewModel
                            {
                                Id = prod.Id,
                                ProductName = prod.Name,
                                CategoryName = prod.Category.Name,
                                SupplierName = prod.Supplier.CompanyName,
                                Description = prod.Description,
                                Price = FormatPrice(prod.Price),
                                Stock = prod.Stock,
                                OnOrder = prod.OnOrder,
                                Discontinue = FormatDiscontinued(prod.Discontinue)
                            }).ToList();
            return products;

        }

        public IndexProductViewModel GetIndex(int page, string searchProduct, string searchCategory, string searchSupplier)
        {
            IEnumerable<GridProductViewModel> products = GetDataIndex();

            int totalKeseluruhan = products.Count();

            if (!string.IsNullOrEmpty(searchProduct))
            {
                products = products.Where(a => a.ProductName.Contains(searchProduct));
            }
            if (!string.IsNullOrEmpty(searchCategory))
            {
                products = products.Where(a => a.CategoryName.Contains(searchCategory));
            }
            if (!string.IsNullOrEmpty(searchSupplier))
            {
                products = products.Where(a => a.SupplierName.Contains(searchSupplier));
            }

            int totalHalaman = (int)Math.Ceiling(products.Count() / (decimal)TotalDataPerPage);
            int totalData = products.Count();
            int skip = (TotalDataPerPage * (page - 1));

            products = products.Skip(skip).Take(TotalDataPerPage);

            var model = new IndexProductViewModel
            {
                SearchCategory = searchCategory,
                SearchProduct = searchProduct,
                SearchSupplier = searchSupplier,
                GridProduct = products,
                TotalKeseluruhan = totalKeseluruhan,
                TotalHalaman = totalHalaman,
                TotalData = totalData
            };

            return model;
        }

        public UpdateViewModel GetDataAdd()
        {
            var model = new UpdateViewModel()
            {
                DropDownSupplier = GetSupplier(),
                DropDownCategory = GetCategory(),
                DropDownCategoryCustom = GetCategoryCustom(),
                DropDownSupplierCustom = GetSupplierCustom()
            };
            return model;
        }

        public void PostAdd(UpdateViewModel model)
        {
            try
            {

                var product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId,
                    SupplierId = model.SupplierId,
                    Price = Convert.ToDecimal(model.Price),
                    Stock = model.Stock,
                    OnOrder = model.OnOrder,
                    Discontinue = model.Discontinue
                };
                ProductRepository.GetRepository().Insert(product);

            }
            catch (Exception)
            {
                throw;
            }

        }

        public UpdateViewModel GetUpdate(long id)
        {
            var oldProduct = ProductRepository.GetRepository().GetSingle(id);
            var model = new UpdateViewModel()
            {
                Id = oldProduct.Id,
                Name = oldProduct.Name,
                Description = oldProduct.Description,
                CategoryId = oldProduct.CategoryId,
                SupplierId = oldProduct.SupplierId,
                Price = oldProduct.Price,
                Stock = oldProduct.Stock,
                OnOrder = oldProduct.OnOrder,
                Discontinue = oldProduct.Discontinue,
                DropDownCategoryCustom = GetCategoryCustom(),
                DropDownSupplierCustom = GetSupplierCustom()
            };

            return model;

        }

        public void PostUpdate(UpdateViewModel model)
        {
            var product = ProductRepository.GetRepository().GetAll().ToList();
            try
            {
                var oldProduct = product.Where(a => a.Id == model.Id).SingleOrDefault();
                MapingModel(oldProduct, model);

                ProductRepository.GetRepository().Update(oldProduct);
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region Function
        public static string FormatPrice(decimal price)
        {
            string formatPrice = price.ToString("C", CultureInfo.CreateSpecificCulture("id-ID"));
            return formatPrice;
        }

        public static string FormatDiscontinued(bool disc)
        {
            string formatDiscontinue = (disc == true) ? "Discontinued" : "Continue";
            return formatDiscontinue;
        }

        public List<SelectListItem> GetSupplier()
        {
            var supplier = SupplierRepository.GetRepository().GetAll().ToList();
            var result = supplier.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.CompanyName
            }).ToList();
            return result;

        }

        public List<DropDownListViewModel> GetSupplierCustom()
        {
            var supplier = SupplierRepository.GetRepository().GetAll().ToList();
            var result = supplier.Select(a => new DropDownListViewModel
            {
                LongValue = a.Id,
                Text = a.CompanyName
            }).ToList();
            return result;


        }

        public List<DropDownListViewModel> GetCategoryCustom()
        {
            var categories = CategoryRepository.GetRepository().GetAll().ToList();
            var result = categories.Select(a => new DropDownListViewModel
            {
                LongValue = a.Id,
                Text = a.Name
            }).ToList();
            return result;


        }

        public List<SelectListItem> GetCategory()
        {
            var categories = CategoryRepository.GetRepository().GetAll().ToList();
            var result = categories.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return result;

        }

        #endregion
    }
}
