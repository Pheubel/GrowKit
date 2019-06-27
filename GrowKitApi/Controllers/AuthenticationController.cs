using GrowKitApi.Services;
using GrowKitApi.Services.Enums;
using GrowKitApiDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace GrowKitApi.Controllers
{
    /// <summary> Contains behavior of endpoints related to authentication.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IUserManagementService _userManagementService;
        private readonly IEmailService _emailService;

        // injects the services into the controller
        public AuthenticationController(IAuthenticateService authenticateService, IUserManagementService userManagementService, IEmailService emailService)
        {
            _authenticateService = authenticateService;
            _userManagementService = userManagementService;
            _emailService = emailService;
        }

        /// <summary> Authenticates the user with the provided credentials.</summary>
        /// <param name="credentials"> The credentials used for authentication.</param>
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticationDTO credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authenticateService.AuthenticateAsync(credentials);

            if (result.Result == AuthenticationResults.Succes)
                return Ok(result.TokenString);
            else
                return BadRequest();
        }

        /// <summary> Verifies the 2fa code sent by the user.</summary>
        /// <param name="authorization"> The authorization header string containing the bearer token.</param>
        /// <param name="code"> The code sent by the user</param>
        [HttpPost("verify2fa/{code}")]
        [AllowAnonymous]
        public async Task<IActionResult> Verify2Fa([FromHeader]string authorization, string code)
        {
            const int BearerMinimumLength = 8;

            if (authorization.Length < BearerMinimumLength)
                return BadRequest();

            if (!authorization.AsSpan(0, BearerMinimumLength - 1).Equals("Bearer ", StringComparison.Ordinal))
                return BadRequest();

            var tokenString = authorization.AsSpan(BearerMinimumLength - 1).ToString();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);

            if (!long.TryParse(token.Subject, out var userId))
                return BadRequest();

            var result = await _authenticateService.ValidateAuthenticationCodeAsync(code, userId);

            if (result.Result == AuthenticationResults.Succes)
                return Ok(result.TokenString);

            return BadRequest();
        }

        /// <summary> Registers a new acount for the user using the given credentials.</summary>
        /// <param name="credentials"> The user credentias used for registration</param>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AuthenticationDTO credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userManagementService.CreateAsync(credentials.Email, credentials.Password);

            if (result.Result != AuthenticationResults.Succes)
                return BadRequest();

            var code = await _userManagementService.GenerateEmailConfiramtionTokenAsync(result.User);

            // The url sent to the user via email
            // This functionality has not been implemented yet.
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = result.User, code = code },
                protocol: Request.Scheme);

            await _emailService.SendEmailAsync(credentials.Email, "Email verification",
               $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            return Ok("Please verify your email adress");
        }
    }
}