using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;

public class HomeController : ControllerBase
{
    // GET: HomeController
    public ActionResult Index()
    {
        return Redirect("~/swagger");
    }
}