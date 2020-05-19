using Microsoft.AspNetCore.Mvc;
using Nadlan.Models.Security;
using Nadlan.Repositories;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly NadlanConext _context;
        private readonly JwtSettings _jwtSettigs;

        public SecurityController(NadlanConext conext, JwtSettings jwtSettings)
        {
            _context = conext;
            _jwtSettigs = jwtSettings;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUserAuth auth = new AppUserAuth();
            SecurityManager securityManager = new SecurityManager(_context, _jwtSettigs);          
           
            auth = securityManager.ValidateUser(user);
            if (auth.IsAuthenticated)
            {
                return StatusCode(200, auth);
            }
            else
            {
                return StatusCode(404, "Invalid username / password.");
            }
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] AppUser user)
        //{
        //    AppUserAuth auth = new AppUserAuth();
        //    SecurityManager securityManager = new SecurityManager(_context);
        //    auth = securityManager.ValidateUser(user);
        //    if (auth.IsAuthenticated)
        //    {
        //        return StatusCode(200, auth);
        //    }
        //    else
        //    {
        //        return StatusCode(404, "Invalid username / password.");
        //    }

        //}

    }


}