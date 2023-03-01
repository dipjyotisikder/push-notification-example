namespace PN.Firestore.Entities
{
    public abstract class BaseEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the id of the user.
        /// </summary>
        public string Id { get; set; }
    }
}
