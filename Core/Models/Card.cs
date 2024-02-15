using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Cards.Enums;

namespace Cards.Models
{
    public class Card : BaseModel
    {
        [Required]
        public string Name { get; set; }

        // Description is optional
        public string Description { get; set; } = string.Empty;

        // Color is optional
        [RegularExpression(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")]
        public string? Color { get; set; }

        public CardStatus Status { get; set; } = CardStatus.ToDo;

        [Required]
        public Guid UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}