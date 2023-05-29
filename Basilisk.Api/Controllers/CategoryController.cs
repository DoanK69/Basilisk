using Basilisk.Provider;
using Basilisk.ViewModel;
using Basilisk.ViewModel.Category;
using Basilisk.ViewModel.Product;
using Microsoft.AspNetCore.Mvc;

namespace Basilisk.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<GridCategoryViewModel> Get()
        {
            var result = CategoryProvider.GetProvider().GetDataIndex("");
            return result;
        }

        [HttpGet]
        [Route("{GetByName}")]
        public IEnumerable<GridCategoryViewModel> Get(string? name, string desc)
        {
            var result = CategoryProvider.GetProvider().GetDataIndex(desc);
            return result;
        }

        [HttpPost("PostAdd/{model}")]
        public IActionResult Post(CreateUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CategoryProvider.GetProvider().PostAdd(model);
                    return Ok($"Data Berhasil ditambah");
                }
                catch (Exception)
                {
                    return Ok($"Data Gagal ditambah");
                }
            }

            return Ok($"Data Gagal ditambah");
        }

        [HttpPut("PutEdit")]
        public IActionResult Edit(CreateUpdateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    CategoryProvider.GetProvider().PostUpdate(model);
                    return Ok($"Data Berhasil diupdate");
                }
                catch (Exception)
                {
                    return Ok($"Data Gagal diupdate");
                }
            }

            return Ok($"Data Gagal diupdate");
        }

        [HttpGet]
        [Route("GetDetailById/{id}")]
        public IEnumerable<GridProductViewModel> Detail(int id)
        {
            var model = CategoryProvider.GetProvider().GetDetail(id);
            return model;
        }

        [HttpGet("GetEditById/{id}")]
        public JsonResultViewModel Edit(long id)
        {
            var model = CategoryProvider.GetProvider().GetUpdateApi(id);
            return model;
        }

        [HttpPatch("PatchEdit/{id}")]
        public string UpdateDescription(long id, [FromBody] string description)
        {
            CategoryProvider.GetProvider().PostUpdate(id, description);
            return $"Data Berhasil diupdate";
        }

        [HttpDelete("{id}")]
        public string Delete(long id)
        {
            var cek = CategoryProvider.GetProvider().GetDelete(id);
            if (!cek)
            {
                CategoryProvider.GetProvider().PostDelete(id);
                return "Delete Berhasil";
            }
            return "Category ini memiliki relasi dengan Produk"; 
        }

    }
}
