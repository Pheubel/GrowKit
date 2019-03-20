using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrowkitDataModels;
using Growkit_website.Data;
using Microsoft.AspNetCore.Authorization;
using Growkit_website.ServerScripts.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Growkit_website.ServerScripts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Growkit_website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenProviderOptions _tokenProvider;
        private readonly ILogger<ApplicationUsersController> _logger;

        public ApplicationUsersController(
            ApplicationDbContext context,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IOptionsSnapshot<TokenProviderOptions> tokenProvider,
            ILogger<ApplicationUsersController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenProvider = tokenProvider.Value;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] ApplicationUser userBody)
        {
            var result = await _userManager.CreateAsync(userBody);

            if (result.Succeeded)
                return Ok();
            else
                return BadRequest(new { errors = result.Errors });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> SignInUser([FromBody]LogInInputModel inputModel)
        {
            var user = await _userManager.FindByEmailAsync(inputModel.Email);

            if (user == null)
            {
                // say that credentials do not match, even if it is a lie
                return BadRequest(new { Message = "Current credentials are invalid." });
            }

            var result = await _signInManager.PasswordSignInAsync(user, inputModel.Password, inputModel.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // ( ´ ∀ `)ノ hooray, the user has completely logged in and recieves a token to verify themself with

                var claims = new[]
                {
                    new Claim("sub", user.Id.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenProvider.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _tokenProvider.Issuer,
                    audience: _tokenProvider.Audience,
                    claims: null,//claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else if (result.RequiresTwoFactor)
            {
                // the user needs to give his OTP to verify himself
                return Redirect("~/Identity/Account/LoginWith2fa");
            }
            else if (result.IsLockedOut)
            {
                // the user was very naughty and is locked out now
                return BadRequest(new { Message = "You have been locked out" });
            }
            else
            {
                // 	(ノ°益°)ノ credentials do not match
                return BadRequest(new { Message = "Current credentials are invalid." });
            }
        }

        [AllowAnonymous]
        [HttpPost("login2fa")]
        public async Task<ActionResult> SignInUserWith2Fa(LogIn2FAInputModel inputModel)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = inputModel.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, inputModel.RememberMe, inputModel.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return Ok();
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return BadRequest();
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                return BadRequest();
            }
        }

        // GET: api/ApplicationUsers
        [HttpGet]
        [Authorize(RoleConstants.Administrator)]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/ApplicationUsers/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetUser(Guid id)
        {
            var applicationUser = await _context.Users.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(new { applicationUser.UserName,applicationUser.Id});
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult> GetMe()
        {
            var appUser = await _userManager.GetUserAsync(User);

            return Ok(new
            {
                appUser.UserName,
                appUser.Id,
                appUser.Email,
            });
        }

        // PUT: api/ApplicationUsers/5
        [HttpPut("{id}")]
        [Authorize(RoleConstants.Administrator)]
        public async Task<IActionResult> PutApplicationUser(Guid id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ApplicationUsers
        [HttpPost]
        [Authorize(RoleConstants.Administrator)]
        public async Task<ActionResult<ApplicationUser>> PostApplicationUser(ApplicationUser applicationUser)
        {
            _context.Users.Add(applicationUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // DELETE: api/ApplicationUsers/5
        [HttpDelete("{id}")]
        [Authorize(RoleConstants.Administrator)]
        public async Task<ActionResult<ApplicationUser>> DeleteApplicationUser(Guid id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return applicationUser;
        }

        private bool ApplicationUserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        public class LogInInputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class LogIn2FAInputModel
        {
            public string TwoFactorCode { get; set; }
            public bool RememberMe { get; set; }
            public bool RememberMachine { get; set; }
        }

    }
}
