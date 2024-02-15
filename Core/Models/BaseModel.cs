namespace Cards.Models
{
    /// <summary>
    /// Represents the base model for all entities in the application.
    /// </summary>
    public class BaseModel : IBaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
       public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the date and time the entity was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the date and time the entity was last updated.
        /// This property is nullable and can be null if the entity has not been updated since creation.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}