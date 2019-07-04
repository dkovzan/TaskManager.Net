using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TaskManager.WEB.ViewModels;

namespace TaskManager.WEB.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IMapper Mapper;

        protected BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }

        public abstract ActionResult List(string searchTerm, string currentFilter, string sortColumn, bool? isAscending, int? page, int? pageSize);

        public abstract ActionResult Delete(int id);

        protected ListView<T> GetListViewPerPageWithPageInfo<T>(List<T> fullEntitiesList, int? page, int? pageSize) where T : class
        {
            var pageNumber = page ?? 1;

            var pageAmount = pageSize ?? 5;

            var pageInfo = new PageInfo { PageNumber = pageNumber, PageSize = pageAmount, TotalItems = fullEntitiesList.Count };

            if (page > pageInfo.TotalPages)
            {
                pageInfo.PageNumber = 1;
            }

            var entitiesPerPage = fullEntitiesList.Skip((pageNumber - 1) * pageAmount).Take(pageAmount).ToList();

            return new ListView<T> { EntitiesPerPageList = entitiesPerPage, PageInfo = pageInfo };
        }

    }
}