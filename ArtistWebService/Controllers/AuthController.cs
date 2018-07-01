using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using DataAccessLayer.Entities;
using ArtistWebService.Data;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ArtistWebService.Controllers
{

   
    public class AuthController : Controller
    {


        private readonly SignInManager<AppUser> signIn;
        private readonly UserManager<AppUser> userManager;
        private readonly IPasswordHasher<AppUser> hasher;
        private IConfiguration Configuration { get; }

        public AuthController(SignInManager<AppUser> _signIn, PasswordHasher<AppUser> _hasher,
            UserManager<AppUser> _userManager, IConfiguration _config)
        {
            signIn = _signIn;
            userManager = _userManager;
            hasher = _hasher;
            Configuration = _config;

        }

        
        [HttpPost("api/auth/login")] 
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await signIn.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                return Ok("Successful login");
            }

            return BadRequest("Failed to login");
        }

        [HttpPost("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] CredentialModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.UserName, Email = model.Email, IsSuperUser= model.IsSuperUser };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok($"Successful: User:{user} has been created ");
                }
            }

            return BadRequest("Failed to login");
        }



        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var claims = new[]
                    {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: Configuration["Auth:Issuer"],
                        audience: Configuration["Auth:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(24),
                        signingCredentials: creds
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
            }



            return BadRequest("Failed to create a token");
        }
    }
}
