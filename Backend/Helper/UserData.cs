using System.Security.Claims;

namespace Backend.Helper
{
    public static class UserData
    {
        public static Guid? GetUserId(HttpContext c)
        {
            var id = Guid.Empty;
            var identity = c.User.Identity as ClaimsIdentity;
            if (identity?.Claims.Count() != 0)
            {
                IEnumerable<Claim> claims = identity!.Claims;
                id = Guid.Parse(claims.First(x => x.Type == ClaimTypes.Sid).Value);
            }
            return id;
        }
    }
}