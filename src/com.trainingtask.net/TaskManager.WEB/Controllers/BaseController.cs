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

        public abstract ActionResult List(int page, int pageSize);

        public abstract ActionResult Delete(int id);

        protected ListView<T> GetListViewPerPageWithPageInfo<T>(List<T> fullEntitiesList, int page, int pageSize) where T : class
        {
            var pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = fullEntitiesList.Count };

            if (page > pageInfo.TotalPages)
            {
                pageInfo.PageNumber = 1;
            }

            var entitiesPerPage = fullEntitiesList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new ListView<T> { EntitiesPerPageList = entitiesPerPage, PageInfo = pageInfo };
        }

    }
}