using System;
using System.Security.Claims;

namespace Suma.Social.Utils
{
    public static class JwtUtil
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return Convert.ToInt32(user.FindFirst("id").Value);
        }
    }
}