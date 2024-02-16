using System.Text.Json.Serialization;
using Cards.Enums;
using Microsoft.AspNetCore.Identity;

namespace Cards.Models
{
    /// <summary>
    /// Represents a User entity in the application.
    /// Inherits from the BaseModel class.
    /// </summary>
    public class User : IdentityUser<Guid>, IBaseModel
    {
        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Role ID Guid
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// Gets or sets the role of the user.
        /// The role can be one of the following: Member, Admin.
        /// </summary>
        public Role Role { get; set; }


        [JsonIgnore] public ICollection<Card> Cards { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}