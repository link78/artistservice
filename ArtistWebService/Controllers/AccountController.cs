using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using ArtistWebService.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> signIn;
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHasher<AppUser> hasher;
        

        public AccountController(SignInManager<AppUser> _signIn, PasswordHasher<AppUser> _hasher,
            UserManager<AppUser> _userManager)
        {
            signIn = _signIn;
            userManager = _userManager;
            hasher = _hasher;
           

        }


        [HttpPost("api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await signIn.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return Ok("Successful login");
            }

            return BadRequest("Failed to login");
        }

        [HttpPost("api/account/register")]
        public async Task<IActionResult> Register([FromBody] CredentialModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email, IsSuperUser = model.IsSuperUser };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok($"Successful: User:{user} has been created ");
                }
            }

            return BadRequest("Failed to login");
        }
    }
}
