using Google.Cloud.Firestore;

namespace PN.Firestore.Entities
{
    /// <summary>
    /// Represents a user that will be stored in firestore.
    /// </summary>
    [FirestoreData]
    public sealed class User : BaseEntity
    {
        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        [FirestoreProperty]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        [FirestoreProperty]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email of the user.
        /// </summary>
        [FirestoreProperty]
        public string Email { get; set; }
    }
}
