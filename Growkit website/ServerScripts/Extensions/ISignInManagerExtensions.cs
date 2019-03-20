using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts.Extensions
{
    public static class ISignInManagerExtensions
    {
        /// <summary> Attempts to sign in the specified <paramref name="email"/> and <paramref name="password"/> 
        /// combination as an asynchronous operation.</summary>
        /// <param name="signInManager"> The API provider for user sign ins.</param>
        /// <param name="email"> The user name to sign in.</param>
        /// <param name="password"> The password to attempt to sign in with.</param>
        /// <param name="isPersistent"> Flag indicating whether the sign-in cookie should persist after the browser is closed.</param>
        /// <param name="lockoutOnFailure"> Flag indicating if the user account should be locked if the sign in fails.</param>
        /// <returns> The task object representing the asynchronous operation containing the <see name="SignInResult"/>
        /// for the sign-in attempt.</returns>
        public static async Task<SignInResult> EmailPasswordSignInAsync<T>(this SignInManager<T> signInManager, string email, string password, bool isPersistent, bool lockoutOnFailure) where T : class
        {
            var user = await signInManager.UserManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }
}
