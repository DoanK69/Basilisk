using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel;
using Basilisk.ViewModel.SalesmanRegions;
using Basilisk.ViewModel.Salesmen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class SalesmenProvider : BaseProvider
    {
        private static SalesmenProvider _instance = new SalesmenProvider();

        public static SalesmenProvider GetProvider()
        {
            return _instance;
        }

        public IEnumerable<GridSalesmenViewModel> GetDataIndex()
        {

            var salesmen = SalesmenRepository.GetRepository().GetAll().ToList();
            var salesmens = (from s in salesmen
                             join sal in salesmen on s.SuperiorEmployeeNumber equals sal.EmployeeNumber
                             into newSales
                             from p in newSales.DefaultIfEmpty()
                             select new GridSalesmenViewModel
                             {
                                 EmployeeNumber = s.EmployeeNumber,
                                 FirstName = s.FirstName,
                                 LastName = s.LastName,
                                 Level = s.Level,
                                 BirthDate = s.BirthDate,
                                 HiredDate = s.HiredDate,
                                 Address = s.Address,
                                 City = s.City,
                                 Phone = s.Phone == null ? "N/A" : s.Phone,
                                 SuperiorEmployeeNumber = (s.SuperiorEmployeeNumber == null) ? "N/A" : p.FirstName
                             }).ToList();

            return salesmens;

        }

        public IndexSalesViewModel GetIndex(int page, string searchName)
        {
            IEnumerable<GridSalesmenViewModel> salesmens = GetDataIndex();

            int totalSales = salesmens.Count();

            if (!string.IsNullOrEmpty(searchName))
            {
                salesmens = salesmens.Where(a => (a.FirstName + " " + a.LastName).Contains(searchName));
            }

            int totalHalaman = (int)Math.Ceiling(totalSales / (decimal)TotalDataPerPage);
            int totalData = salesmens.Count();
            int skip = (TotalDataPerPage * (page - 1));

            salesmens = salesmens.Skip(skip).Take(TotalDataPerPage);

            var model = new IndexSalesViewModel()
            {
                SearchName = searchName,
                GridSales = salesmens,
                TotalSales = totalSales,
                TotalHalaman = totalHalaman,
                TotalData = totalData
            };

            return model;
        }

        private List<DropDownListViewModel> GetSuperior()
        {
            var salesmen = SalesmenRepository.GetRepository().GetAll().ToList();
            var result = salesmen.Select(a => new DropDownListViewModel
            {
                StringValue = a.EmployeeNumber,
                Text = a.FirstName,
            }).ToList();

            return result;

        }

        public UpsertSalesmenViewModel GetAdd()
        {
            var model = new UpsertSalesmenViewModel()
            {
                DropDownSuperiorEmployeeNumber = GetSuperior()
            };

            return model;
        }

        public void PostAdd(UpsertSalesmenViewModel model)
        {

            try
            {
                var sales = new Salesman()
                {
                    EmployeeNumber = model.EmployeeNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Level = model.Level,
                    BirthDate = model.BirthDate,
                    HiredDate = model.HiredDate,
                    Address = model.Address,
                    City = model.City,
                    Phone = model.Phone,
                    SuperiorEmployeeNumber = model.SuperiorEmployeeNumber
                };

                SalesmenRepository.GetRepository().Insert(sales);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public UpsertSalesmenViewModel DropDownSupplier(UpsertSalesmenViewModel model)
        {
            model.DropDownSuperiorEmployeeNumber = GetSuperior();
            return model;
        }

        public UpsertSalesmenViewModel GetUpdate(string empNumber)
        {

            var salesmen = SalesmenRepository.GetRepository().GetSingle(empNumber);
            var oldSales = salesmen;
            var model = new UpsertSalesmenViewModel()
            {
                EmployeeNumber = empNumber,
                FirstName = oldSales.FirstName,
                LastName = oldSales.LastName,
                Level = oldSales.Level,
                BirthDate = oldSales.BirthDate,
                HiredDate = oldSales.HiredDate,
                Address = oldSales.Address,
                City = oldSales.City,
                Phone = oldSales.Phone,
                SuperiorEmployeeNumber = oldSales.SuperiorEmployeeNumber,
                DropDownSuperiorEmployeeNumber = GetSuperior()
            };
            return model;

        }

        public void PostUpdate(UpsertSalesmenViewModel model)
        {
            var salesmen = SalesmenRepository.GetRepository().GetAll().ToList();
            try
            {
                var oldSales = salesmen.Where(a => a.EmployeeNumber == model.EmployeeNumber).SingleOrDefault();
                MapingModel(oldSales, model);

                SalesmenRepository.GetRepository().Update(oldSales);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public DetailSalesmanViewModel GetDetail(string empNumber)
        {
            var salesmen = SalesmenRepository.GetRepository().GetAll().ToList();
            var salesmenRegion = SalesmenRegionRepository.GetRepository().GetAll().ToList();
            var region = RegionRepository.GetRepository().GetAll().ToList();
            var sales = (from sal in salesmen
                         where sal.EmployeeNumber == empNumber
                         select new GridSalesmenViewModel
                         {
                             EmployeeNumber = sal.EmployeeNumber,
                             FirstName = sal.FirstName,
                             LastName = sal.LastName,
                             Level = sal.Level
                         }).SingleOrDefault();

            var regions = (from sal in salesmen
                           join salReg in salesmenRegion on sal.EmployeeNumber equals salReg.SalesmanEmployeeNumber
                           join reg in region on salReg.RegionId equals reg.Id
                           where sal.EmployeeNumber == empNumber
                           select new GridSalesmanRegionsViewModel
                           {
                               EmployeeNumber = sal.EmployeeNumber,
                               FirstName = sal.FirstName,
                               LastName = sal.LastName,
                               Level = sal.Level,
                               RegionId = reg.Id,
                               RegionCity = reg.City,
                               RegionRemark = reg.Remark
                           }).ToList();

            var model = new DetailSalesmanViewModel()
            {
                GridSales = sales,
                GridRegions = regions,
                TotalRegion = regions.Count()
            };

            return model;

        }


        public void PostDelete(string empNumber)
        {
            SalesmenRepository.GetRepository().GetSingle(empNumber);
            SalesmenRepository.GetRepository().Delete(empNumber);
        }
    }
}
