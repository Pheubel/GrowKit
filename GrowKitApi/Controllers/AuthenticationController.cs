using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using GrowKitApi.Services;
using GrowKitApi.Services.Enums;
using GrowKitApiDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrowKitApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;
        private readonly IUserManagementService _userManagementService;
        private readonly IEmailService _emailService;

        public AuthenticationController(IAuthenticateService authenticateService, IUserManagementService userManagementService, IEmailService emailService)
        {
            _authenticateService = authenticateService;
            _userManagementService = userManagementService;
            _emailService = emailService;
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInAsync([FromBody] AuthenticationDTO credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _authenticateService.AuthenticateAsync(credentials);

            if (result.Result == Results.Succes)
                return Ok(result.TokenString);
            else
                return BadRequest();
        }

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

            if (result.Result == Results.Succes)
                return Ok(result.TokenString);

            return BadRequest();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] AuthenticationDTO credentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userManagementService.CreateAsync(credentials.Email, credentials.Password);

            if (result.Result != Results.Succes)
                return BadRequest();

            var code = await _userManagementService.GenerateEmailConfiramtionTokenAsync(result.User);
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