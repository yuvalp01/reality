using Microsoft.IdentityModel.Tokens;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;

namespace Nadlan.Models.Security
{
    public class SecurityManager : Repository<AppUser>
    {
        private readonly JwtSettings _jwtSettings = null;
        public SecurityManager(NadlanConext conext, JwtSettings jwtSettings) : base(conext)
        {
            _jwtSettings = jwtSettings;
        }

        public AppUserAuth ValidateUser(AppUser user)
        {

            AppUser authUser = Context.Users.Where(a => a.UserName == user.UserName
        && a.Password == user.Password).FirstOrDefault();
            if (authUser != null)
            {
                return BuildUserAuthObject(authUser);
            }
            else return new AppUserAuth();

        }
        protected List<AppUserClaim> GetUserClaims(AppUser user)
        {
            List<AppUserClaim> claims = new List<AppUserClaim>();
            try
            {
                claims = Context.Claims.Where(c => c.UserId == user.UserId).ToList();
            }
            catch (Exception ex)
            {

                throw new Exception("Exception trying to retrieve user claims", ex);
            }
            return claims;
        }

        protected string BuildJwtToken(AppUserAuth authUser)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.Key));

            List<Claim> jwtClaims = new List<Claim>();
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, authUser.UserName));
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            jwtClaims.Add(new Claim("isAuthenticated", authUser.IsAuthenticated.ToString().ToLower()));
            foreach (var claim in authUser.Claims)
            {
                jwtClaims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }
           
            //jwtClaims.Add(new Claim("canAccessRest", authUser.CanAccessRest.ToString().ToLower()));
            //jwtClaims.Add(new Claim("canConfirmExpenses", authUser.CanConfirmExpenses.ToString().ToLower()));
            //jwtClaims.Add(new Claim("canViewExpenses", authUser.CanViewExpenses.ToString().ToLower()));
            //jwtClaims.Add(new Claim("canViewReports", authUser.CanViewReports.ToString().ToLower()));
            //jwtClaims.Add(new Claim("canViewTransactions", authUser.CanViewTransactions.ToString().ToLower()));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: jwtClaims,
                notBefore: DateTime.UtcNow, 
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.MinutesToExpiration),
                signingCredentials: new SigningCredentials(key,SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);     

        }



        protected AppUserAuth BuildUserAuthObject(AppUser user)
        {
            AppUserAuth auth = new AppUserAuth();
            List<AppUserClaim> claims = new List<AppUserClaim>();
            auth.UserName = user.UserName;
            auth.IsAuthenticated = true;
            auth.BearerToken = new Guid().ToString();
            auth.Claims = GetUserClaims(user);
            //claims = GetUserClaims(user);
            //foreach (var claim in claims)
            //{
            //    try
            //    {
            //        PropertyInfo claimProperty = typeof(AppUserAuth).GetProperty(claim.ClaimType);
            //        claimProperty.SetValue(auth, Convert.ToBoolean(claim.ClaimValue), null);
            //    }
            //    catch { }
            //}

            auth.BearerToken = BuildJwtToken(auth);

            return auth;
        }
    }
}
