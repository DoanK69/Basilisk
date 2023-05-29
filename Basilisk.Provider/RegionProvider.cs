using Basilisk.DataAccess.Models;
using Basilisk.Repository;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Region;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basilisk.Provider
{
    public class RegionProvider : BaseProvider
    {
        private static RegionProvider _intance = new RegionProvider();

        public static RegionProvider GetProvider()
        {
            return _intance;
        }

        public IEnumerable<GridRegionViewModel> GetDataIndex()
        {
            var region = (from reg in RegionRepository.GetRepository().GetAll()
                          select new GridRegionViewModel
                          {
                              Id = reg.Id,
                              City = reg.City,
                              Remark = reg.Remark
                          }).ToList();
            return region;
        }

        public IndexRegionViewModel GetIndex(int page, string searchCity)
        {
            IEnumerable<GridRegionViewModel> data = GetDataIndex();

            if (!string.IsNullOrEmpty(searchCity))
            {
                data = data.Where(a => a.City.Contains(searchCity));
            }

            int totalRegion = data.Count();

            int totalHalaman = (int)Math.Ceiling(data.Count() / (decimal)TotalDataPerPage);

            int skip = (TotalDataPerPage * (page - 1));

            data = data.Skip(skip).Take(TotalDataPerPage);
            var model = new IndexRegionViewModel()
            {
                SearchCity = searchCity,
                GridRegion = data,
                TotalRegion = totalRegion,
                TotalData = data.Count(),
                TotalHalaman = totalHalaman,
            };

            return model;
        }

        public UpsertRegionViewModel GetEdit(long idRegion)
        {
            var oldRegion = RegionRepository.GetRepository().GetSingle(idRegion);
            var model = new UpsertRegionViewModel
            {
                Id = idRegion,
                City = oldRegion.City,
                Remark = oldRegion.Remark
            };
            return model;
        }

        public void Save(UpsertRegionViewModel model)
        {

            if (model.Id == 0)
            {
                var region = new Region();
                MapingModel<Region, UpsertRegionViewModel>(region, model);
                RegionRepository.GetRepository().Insert(region);
            }
            else
            {
                var oldRegion = RegionRepository.GetRepository().GetSingle(model.Id);
                MapingModel<Region, UpsertRegionViewModel>(oldRegion, model);

                RegionRepository.GetRepository().Update(oldRegion);

            }

        }

        public void Delete(long idRegion)
        {

                try
                {
                    var entityModel = RegionRepository.GetRepository().GetSingle(idRegion);
                    RegionRepository.GetRepository().Delete(idRegion);
                }
                catch
                {
                    throw;
                }
            
        }
    }
}
