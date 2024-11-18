using Microsoft.AspNetCore.Mvc;

namespace ClothesDG.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        [HttpGet("/Administration")]
        public IActionResult Index()
        {
            ViewData["IsHomePage"] = false;
            return View();
        }
    }
}
