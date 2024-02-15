using Microsoft.AspNetCore.Identity;

namespace Cards.Enums
{
    public class Role : IdentityRole<Guid>
    {
        public AccessLevel AccessLevel { get; set; }
    }
}