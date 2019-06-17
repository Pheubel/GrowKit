using GrowKitApi.Contexts;
using GrowKitApi.Entities;
using GrowKitApi.Entities.Enums;
using GrowKitApi.Services.Enums;
using GrowKitApi.Services.Structs;
using Microsoft.EntityFrameworkCore;
using OtpNet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    public class UserManagementService : IUserManagementService
    {
        private IdentityContext _context;

        public UserManagementService(IdentityContext context)
        {
            _context = context;
        }

        public async Task<IdentityUser> FindUserById(ulong id) => await _context.Users.FindAsync(id);
        public async Task<IdentityUser> FindUserByEmail(string email) => await _context.Users.AsTracking().FirstOrDefaultAsync(u => u.NormalizedEmailAdress.Equals(email, StringComparison.OrdinalIgnoreCase));

        public async Task<UserResult> ValidateUserAsync(string emailAdress, string password)
        {
            var user = await _context.Users
                .Select(u => new { u.Id, u.NormalizedEmailAdress, u.PasswordHash, u.PasswordSalt, u.Flags }).AsNoTracking()
                .FirstOrDefaultAsync(u => u.NormalizedEmailAdress.Equals(emailAdress, StringComparison.OrdinalIgnoreCase));

            if (user == null || string.IsNullOrWhiteSpace(password) || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return new UserResult(Results.InvalidCredentials);

            return new UserResult(user.Id, user.Flags.HasFlag(IdentityUserFlags.Uses2FA));
        }

        public async Task<UserResult> CreateAsync(string emailAdress, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                return new UserResult(Results.MissingPassword);

            if (await _context.Users.AnyAsync(u => u.NormalizedEmailAdress.Equals(emailAdress, StringComparison.OrdinalIgnoreCase)))
                new UserResult(Results.EmailTaken);

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new IdentityUser(emailAdress)
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return new UserResult(user.Id);
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public async Task<Results> ValidateAuthenticationCodeAsync(string code, long userId)
        {
            var user = await _context.Users.Select(u => new AuthenticationInfo
            {
                Id = u.Id,
                AuthenticatorSecret = u.AuthenticatorSecret,
                PreviousTimeWindow = u.PreviousTimeWindow
            }).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return Results.UnknownError;

            var totpHandler = new Totp(user.AuthenticatorSecret);
            if (!totpHandler.VerifyTotp(code, out long timeWindow))
                return Results.InvalidToken;
            if (user.PreviousTimeWindow == timeWindow)
                return Results.TokenHasBeenUsed;

            user.PreviousTimeWindow = timeWindow;
            _context.Entry(user).Property(u => u.PreviousTimeWindow).IsModified = true;

            return Results.Succes;
        }

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

        private class AuthenticationInfo
        {
            public long Id { get; set; }
            public byte[] AuthenticatorSecret { get; set; }
            public long PreviousTimeWindow { get; set; }
        }
    }
}
