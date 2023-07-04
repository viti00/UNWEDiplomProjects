using System.Security.Claims;

namespace CarPartsShop.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetId(this ClaimsPrincipal user)
        {
            string id = null;

            if (user.Identity.IsAuthenticated == true)
            {
                id = user.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            return id;

        }

    }
}
