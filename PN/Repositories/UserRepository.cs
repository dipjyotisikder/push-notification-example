using Google.Cloud.Firestore;
using PN.Firestore.Entities;

namespace PN.Firestore.Repositories
{
    public class UserRepository : FirebaseRepository<User>, IUserRepository
    {
        public UserRepository(FirestoreDb firestoreDb) : base(firestoreDb)
        {
        }
    }
}
