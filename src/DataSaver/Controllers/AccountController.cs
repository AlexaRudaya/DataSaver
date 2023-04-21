namespace DataSaver.Controllers
{
    public class AccountController : Controller
    {
        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult Login(string? returnUrl = null)
        //{
        //    LoginDto loginDto = new();
        //    loginDto.ReturnUrl = returnUrl ?? Url.Content("~/");

        //    return View(loginDto);
        //}

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Register(RegisterViewModel registerVM)
        //{
        //    var data = new
        //    {
        //        Email = registerVM.Email,
        //        UserName = registerVM.UserName,
        //        Password = registerVM.Password
        //    };
        //    return View(data);
        //}
    }
}
