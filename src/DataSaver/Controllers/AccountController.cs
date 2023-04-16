namespace DataSaver.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Login(string? returnUrl = null)
        //{
        //    LoginDto loginDto = new();
        //    loginDto.ReturnUrl = returnUrl ?? Url.Content("~/");

        //    return View(loginDto);
        //}

        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View(); 
        //}

        //[HttpPost]
        //public IActionResult Register(string? returnUrl = null)
        //{
        //    RegisterDto registerDto = new();
        //    registerDto.ReturnUrl = returnUrl;

        //    return LocalRedirect(returnUrl); 
        //}
    }
}
