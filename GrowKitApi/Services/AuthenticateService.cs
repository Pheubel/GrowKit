using GrowKitApi.Services.Enums;
using GrowKitApi.Services.Structs;
using GrowKitApi.SettingModels;
using GrowKitApiDTO;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    /// <summary> A authentication store implementation that handles common actions related to authentication of users.</summary>
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly TokenSettings _tokenSettings;

        public AuthenticateService(IUserManagementService userManagementService, IOptions<TokenSettings> tokenSettings)
        {
            _userManagementService = userManagementService;
            _tokenSettings = tokenSettings.Value;
        }

        /// <summary> Authenticates the user with the given credentials.</summary>
        /// <param name="credentials"> The user credentials.</param>
        /// <returns> Authentication result.</re
        public async Task<AuthenticationResult> AuthenticateAsync(AuthenticationDTO credentials)
        {
            var userValidation = await _userManagementService.ValidateUserAsync(credentials.Email, credentials.Password);

            if (userValidation.Result != AuthenticationResults.Succes)
                return new AuthenticationResult(userValidation.Result);

            DateTime now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userValidation.User.ToString()),
                new Claim("authorized", (!userValidation.Has2FaEnabled)? "true" : "false")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var jwtCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_tokenSettings.AccesExpiration),
                signingCredentials: jwtCredentials
                );

            var authenticationResult = new AuthenticationResult(new JwtSecurityTokenHandler().WriteToken(jwt));

            return authenticationResult;
        }

        /// <summary> Validates the 2fa code given by the user.</summary>
        /// <param name="code"> The given c.ode.</param>
        /// <param name="userId"> The user to validate agains.</param>
        /// <returns> Authentication result</returns>
        public async Task<AuthenticationResult> ValidateAuthenticationCodeAsync(string code, long userId)
        {
            var result = await _userManagementService.ValidateAuthenticationCodeAsync(code, userId);

            if (result != AuthenticationResults.Succes)
                return new AuthenticationResult(result);

            DateTime now = DateTime.UtcNow;

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim("authorized", "true")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
            var jwtCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _tokenSettings.Issuer,
                audience: _tokenSettings.Audience,
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_tokenSettings.AccesExpiration),
                signingCredentials: jwtCredentials
                );

            var authenticationResult = new AuthenticationResult(new JwtSecurityTokenHandler().WriteToken(jwt));

            return authenticationResult;
        }
    }
}
