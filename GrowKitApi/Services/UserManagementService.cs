using GrowKitApi.Contexts;
using GrowKitApi.Entities;
using GrowKitApi.Entities.Enums;
using GrowKitApi.Services.Enums;
using GrowKitApi.Services.Structs;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    /// <summary> A user store implementation that wil communicate to the database to handle common actions executed upon a user.</summary>
    public class UserManagementService : IUserManagementService
    {
        /// <summary> The context handling user authentication related subjects.</summary>
        private AuthenticationContext _context;

        public UserManagementService(AuthenticationContext context)
        {
            _context = context;
        }

        /// <summary> Fetches the user stored in the authentication context by looking for a matching id.</summary>
        /// <param name="id"> The id used to find the user.</param>
        /// <returns> Instance of the authentication user stored in the database.</returns>
        public async Task<AuthenticationUser> FindUserById(ulong id) => await _context.Users.FindAsync(id);
        /// <summary> Fetches the user stored in the authentication context by looking for a matching email adress.</summary>
        /// <param name="email"> The email used to find the user.</param>
        /// <returns> Instance of the authentication user stored in the database.</returns>
        public async Task<AuthenticationUser> FindUserByEmail(string email) => await _context.Users.AsTracking().FirstOrDefaultAsync(u => u.NormalizedEmailAdress.Equals(email, StringComparison.OrdinalIgnoreCase));

        /// <summary> Validates the current user by comparing the email adress against the password hash storedin the database.</summary>
        /// <param name="emailAdress"> The registered email adress used for user validation.</param>
        /// <param name="password"> The password string that will be compared against the hash stored in the database.</param>
        /// <returns> Result struct indicating the status of the validation request.</returns>
        public async Task<UserResult> ValidateUserAsync(string emailAdress, string password)
        {
            var user = await _context.Users
                .Select(u => new { u.Id, u.NormalizedEmailAdress, u.PasswordHash, u.PasswordSalt, u.Flags }).AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmailAdress.Equals(emailAdress, StringComparison.OrdinalIgnoreCase));

            if (user == null || string.IsNullOrWhiteSpace(password) || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return new UserResult(AuthenticationResults.InvalidCredentials);

            return new UserResult(user.Id, user.Flags.HasFlag(IdentityUserFlags.Uses2FA));
        }

        /// <summary> Inserts the user into the database with the given authentication information.</summary>
        /// <param name="emailAdress"> The email adress used for validation.</param>
        /// <param name="password"> The password that will be used for validation.</param>
        /// <returns> Result struct indicating the status of the validation request.</returns>
        public async Task<UserResult> CreateAsync(string emailAdress, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                return new UserResult(AuthenticationResults.MissingPassword);

            if (await _context.Users.AnyAsync(u => u.NormalizedEmailAdress.Equals(emailAdress, StringComparison.OrdinalIgnoreCase)))
                new UserResult(AuthenticationResults.EmailTaken);

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new AuthenticationUser(emailAdress)
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserResult(user.Id);
        }

        /// <summary> Creates a password hash from the password string that will be stored in the database.</summary>
        /// <param name="password"> The password string provided by the appication user.</param>
        /// <param name="passwordHash"> The result of hashing the password string with salting.</param>
        /// <param name="passwordSalt"> The salt used for hashing the password.</param>
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary> Verifies the given password against the password hash and salt.</summary>
        /// <param name="password"> The password provided by the user.</param>
        /// <param name="storedHash"> The hash stored in the database.</param>
        /// <param name="storedSalt"> The salt stored in the database.</param>
        /// <returns> true if validation is succesful, false if the validation does not match.</returns>
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            // compare the password against the hash by generating a hashed version of the password with the salt.
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    // if the byte doesn't match, exit early.
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            // the assword has succesfully been validated
            return true;
        }

        /// <summary> Validates the authenticator code provided by the user.</summary>
        /// <param name="code"> The code generated by the user's authenticator application.</param>
        /// <param name="userId"> The id of the user issueing the validation request.</param>
        /// <returns> Result struct indicating the status of the validation request.</returns>
        public async Task<AuthenticationResults> ValidateAuthenticationCodeAsync(string code, long userId)
        {
            var user = await _context.Users.Select(u => new AuthenticationInfo
            {
                Id = u.Id,
                AuthenticatorSecret = u.AuthenticatorSecret,
                PreviousTimeWindow = u.PreviousTimeWindow
            }).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return AuthenticationResults.UnknownError;

            var totpHandler = new Totp(user.AuthenticatorSecret);
            if (!totpHandler.VerifyTotp(code, out long timeWindow))
                return AuthenticationResults.InvalidToken;
            if (user.PreviousTimeWindow == timeWindow)
                return AuthenticationResults.TokenHasBeenUsed;

            user.PreviousTimeWindow = timeWindow;
            _context.Entry(user).Property(u => u.PreviousTimeWindow).IsModified = true;

            return AuthenticationResults.Succes;
        }

        /// <summary> Creates a confirmation token that will be used to verify a user's email adress and stores it in the database.</summary>
        /// <param name="userId"> The user ascociated with the email confirmation.</param>
        /// <returns> Token that will be sent to the user's email adress to verify their email adress.</returns>
        public async Task<string> GenerateEmailConfiramtionTokenAsync(long userId)
        {
            Guid token = new Guid();

            var emailRequest = new EmailConfirmationRequest()
            {
                UserId = userId,
                ConfirmationToken = token,
                ExpirationDate = DateTime.UtcNow.AddDays(7)
            };

            await _context.EmailConfirmationRequests.AddAsync(emailRequest);
            await _context.SaveChangesAsync();

            return token.ToString();
        }

        /// <summary> Gets the user id from the given claims principal.</summary>
        /// <param name="claimPrincipal"> The claims principal sent by the user client.</param>
        /// <returns> user id.</returns>
        public long GetUserID(ClaimsPrincipal cp)
        {
            return long.Parse(cp.FindFirstValue(JwtRegisteredClaimNames.Sub));
        }
        private class AuthenticationInfo
        {
            public long Id { get; set; }
            public byte[] AuthenticatorSecret { get; set; }
            public long PreviousTimeWindow { get; set; }
        }
    }
}
