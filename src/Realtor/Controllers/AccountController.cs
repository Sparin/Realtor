using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Realtor.Model.DTO.Customer;
using Realtor.Model.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Realtor.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<Customer> userManager,
            SignInManager<Customer> signInManager,
            IMapper mapper,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        // POST api/<controller>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegistrationForm requestDto)
        {
            _logger.LogInformation($"Registartion request started");
            if (ModelState.IsValid)
            {
                var user = new Customer { UserName = requestDto.Username };
                var result = await _userManager.CreateAsync(user, requestDto.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User created a new account {requestDto.Username}");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return NoContent();
                }
                _logger.LogWarning("Errors occured while creating new user");
                foreach (var error in result.Errors)
                    ModelState.AddModelError("general", error.Description);
            }

            var errors = ModelState.FormatModelErrors();
            _logger.LogWarning($"Registration did not pass validation", errors);
            return BadRequest(errors);
        }

        // POST api/<controller>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginForm requestDto)
        {
            _logger.LogInformation($"Log in request started");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(requestDto.Username, requestDto.Password, requestDto.RememberLogin, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User was authenticated as {requestDto.Username}");
                    return NoContent();
                }

                ModelState.AddModelError("general", "Invalid username or password");
            }

            var errors = ModelState.FormatModelErrors();
            _logger.LogWarning($"Registration was not successful", errors);
            return BadRequest(errors);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            if (User?.Identity.IsAuthenticated == true)
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"{User.Identity.Name} proceed a log out");
            }
            else
                _logger.LogWarning("Not authenticated user proceed a log out");

            return NoContent();
        }
    }
}
