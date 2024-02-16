using System.ComponentModel.DataAnnotations;

namespace Cards.Core.DTOs;

public class UserDto
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// This field is required and should be a valid email address.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// This field is required and should be at least 6 characters long.
    /// </summary>
    [Required]
    [MinLength(6)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }
}