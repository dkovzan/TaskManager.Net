using AutoMapper;
using log4net;
using System.Reflection;
using System.Web.Mvc;
using TaskManager.BLL.Services;

namespace TaskManager.WEB.Controllers
{
    [Authorize]
    public class ExportController : BaseController
    {
        private readonly ILog _logger;
        private readonly ExportService _exportService;

        public ExportController(ExportService exportService, IMapper mapper) : base(mapper)
        {
            _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            _exportService = exportService;
        }

        [Route("Export/{code}")]
        public ActionResult GetReport(string code)
        {
            _logger.Info($"GET Export/{code}");

            WriteReportToResponse(_exportService.GenerateReport(code));

            _logger.Info($"Report generated succesfully.");

            return RedirectToAction(controllerName: code, actionName: "List");
        }
    }
}