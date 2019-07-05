using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using TaskManager.WEB.Helpers;
using TaskManager.WEB.ViewModels;

namespace TaskManager.WEB.Controllers
{
    public class BaseController : Controller
    {
        protected IMapper Mapper;

        protected BaseController(IMapper mapper)
        {
            Mapper = mapper;
        }

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName;

            var cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
                cultureName = cultureCookie.Value;
            else
                cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
                    Request.UserLanguages[0] : 
                    null;

            cultureName = CultureHelper.GetImplementedCulture(cultureName);

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

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