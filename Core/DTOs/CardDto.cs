using Cards.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cards.Core.DTOs
{
    /// <summary>
    /// Represents a Card Data Transfer Object (DTO).
    /// </summary>
    public class CardDto
    {
        /// <summary>
        /// Gets or sets the unique identifier for the card.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the card.
        /// This field is required.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the card.
        /// This field is optional.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the color of the card.
        /// This field is optional and should be in hexadecimal format.
        /// </summary>
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the status of the card.
        /// </summary>
        public CardStatus Status { get; set; }
    }
}