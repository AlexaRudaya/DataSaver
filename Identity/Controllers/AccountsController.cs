namespace Identity_API.Controllers
{
    [Route("api/Accounts")]
    [ApiController]
    public sealed class AccountsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<AccountsController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager; 
            _logger = logger;
        }

        [HttpGet]
        [Route("IsRegistered")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> IsRegistered()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {
                _logger.LogInformation($"User is registered.");

                return Ok("User exists");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto is null)
            {
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.UserName!, loginDto.Password!, loginDto.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    identity.AddClaim(new Claim(ClaimTypes.Name, loginDto.UserName));

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = loginDto.RememberMe
                    };

                    //var user = await _userManager.GetUserAsync(User);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

                    _logger.LogInformation($"User {loginDto.UserName} logged in successfully.");

                    return Ok("Successfully logged in!");
                }
                else
                {
                    _logger.LogWarning($"Invalid login attempt: Provided data for user {loginDto.UserName} is invalid.");

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    
                    return BadRequest(ModelState);
                }
            }

            return NoContent(); 
        }

        [HttpPost]
        [Route("LogOff")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerAUser)
        { 
            if (registerAUser is null) 
            {
                return BadRequest(ModelState);
            }

            if (ModelState.IsValid)
            {
                var user = new AppUser { Email = registerAUser.Email, UserName = registerAUser.UserName };
                var result = await _userManager.CreateAsync(user, registerAUser.Password!);

                if (result.Succeeded)
                {
                    _logger.LogInformation($"User {registerAUser.Email} registered successfully.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok("Successfully registered!");
                }
                else
                {
                    _logger.LogWarning($"Invalid register attempt: User {registerAUser.UserName} entered the password that is not unique enough.");

                    ModelState.AddModelError("Password", "User could not created. Password is not unique enough.");

                    return BadRequest(ModelState);
                }
            }

            return NoContent();
        }
    }
}
