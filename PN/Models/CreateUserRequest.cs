using Google.Cloud.Firestore;

namespace PN.Models
{
    public class CreateUserRequest
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
