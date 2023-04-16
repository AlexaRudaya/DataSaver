﻿namespace Identity_API.Controllers
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

        [HttpPost]
        [Route("Login")]
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
                    _logger.LogInformation($"User {loginDto.UserName} logged in successfully.");

                    return Ok("Successfully logged in!");
                }
                else
                {
                    _logger.LogWarning($"Invalid login attempt: Provided data for user {loginDto.UserName} is invalid.");

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
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
        [ProducesResponseType(201)]
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
                    _logger.LogInformation($"User {registerAUser.UserName} registered successfully.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return Ok("Successfully registered!");
                }
                else
                {
                    _logger.LogWarning($"Invalid register attempt: User {registerAUser.UserName} entered the password that is not unique enough.");

                    ModelState.AddModelError("Password", "User could not created. Password is not unique enough.");
                }
            }

            return NoContent();
        }
    }
}