namespace DataSaver.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}