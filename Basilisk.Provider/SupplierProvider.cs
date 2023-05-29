using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel.Product;
using Basilisk.ViewModel.Supplier;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class SupplierProvider : BaseProvider
    {
        private static SupplierProvider _instance = new SupplierProvider();

        public static SupplierProvider GetProvider()
        {
            return _instance;
        }

        public IEnumerable<GridSupplierViewModel> GetDataIndex()
        {
            using (BasiliskTFContext _context = new BasiliskTFContext())
            {
                var supplier = SupplierRepository.GetRepository().GetAll();
                var suppliers = (from sup in supplier
                                 where sup.DeleteDate == null
                                 select new GridSupplierViewModel
                                 {
                                     Id = sup.Id,
                                     CompanyName = sup.CompanyName,
                                     ContactPerson = sup.ContactPerson,
                                     JobTitle = sup.JobTitle,
                                     Address = sup.Address,
                                     City = sup.City,
                                     Email = sup.Email,
                                     Phone = sup.Phone
                                 }).ToList();

                return suppliers;
            }
        }

        public IndexSupplierViewModel GetIndex(int page, string searchName)
        {
            IEnumerable<GridSupplierViewModel> suppliers = GetDataIndex();

            int totalSupplier = suppliers.Count();

            if (!string.IsNullOrEmpty(searchName))
            {
                suppliers = suppliers.Where(a => a.CompanyName.Contains(searchName));
            }

            int totalHalaman = (int)Math.Ceiling(suppliers.Count() / (decimal)TotalDataPerPage);
            int totalData = suppliers.Count();
            int skip = (TotalDataPerPage * (page - 1));

            suppliers = suppliers.Skip(skip).Take(TotalDataPerPage);

            var model = new IndexSupplierViewModel
            {
                SearchName = searchName,
                Grid = suppliers,
                TotalData = totalData,
                TotalHalaman = totalHalaman,
                TotalSupplier = totalSupplier,
                CurrentPage = page
            };

            return model;
        }

        public DetailSupplierViewModel GetDetail(int id)
        {

            var indoFormat = CultureInfo.CreateSpecificCulture("id-ID");
            var product = ProductRepository.GetRepository().GetAll().ToList();
            var supplier = SupplierRepository.GetRepository().GetAll();
            var result = new GridSupplierViewModel();
            var products = (from prod in product
                            join sup in supplier.ToList() on prod.SupplierId equals sup.Id
                            where sup.Id == id
                            select new GridProductViewModel
                            {
                                Id = prod.Id,
                                ProductName = prod.Name,
                                SupplierName = sup.CompanyName,
                                Description = prod.Description,
                                Price = prod.Price.ToString("C", indoFormat),
                                Stock = prod.Stock,
                                Discontinue = prod.Discontinue ? "Discontinue" : "Continue"
                            }).ToList();

            var suppliers = (from sup in supplier.ToList()
                             where sup.Id == id
                             select new GridSupplierViewModel
                             {
                                 Id = sup.Id,
                                 CompanyName = sup.CompanyName,
                                 ContactPerson = sup.ContactPerson,
                                 City = sup.City
                             }).SingleOrDefault();

            var model = new DetailSupplierViewModel
            {
                Supplier = suppliers,
                GridProducts = products
            };

            return model;

        }

        public void PostAdd(CreateUpdateSupplierViewModel model)
        {

            try
            {
                var entity = new Supplier();
                MapingModel(entity, model);
                entity.Email = model.Email == null ? "N/A" : model.Email;

                SupplierRepository.GetRepository().Insert(entity);
            }
            catch (Exception)
            {
                throw;
            }



        }
        public bool GetDelete(long id)
        {

            bool anyDependentProduct = ProductRepository.GetRepository().GetAll().Any(product => product.SupplierId == id);

            return anyDependentProduct;

        }

        public void PostDelete(long id)
        {

            var supplier = SupplierRepository.GetRepository().GetSingle(id);
            SupplierRepository.GetRepository().Delete(id);
            supplier.DeleteDate = DateTime.Now;

        }

        public CreateUpdateSupplierViewModel GetEdit(long id, int page)
        {

                var model = new CreateUpdateSupplierViewModel();
                var oldSupplier = SupplierRepository.GetRepository().GetSingle(id);
                MapingModel(model, oldSupplier);
                model.CurrentPage = page;

                return model;
            
        }

        public void PostEdit(CreateUpdateSupplierViewModel model)
        {

            try
            {
                var oldSupplier = SupplierRepository.GetRepository().GetSingle(model.Id);
                MapingModel(oldSupplier, model);
                SupplierRepository.GetRepository().Update(oldSupplier);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
