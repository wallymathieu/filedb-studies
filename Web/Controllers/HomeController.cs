using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController:Controller
    {
        public ActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}