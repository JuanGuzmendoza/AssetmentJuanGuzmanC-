using Hospital.Models;

namespace Hospital.Repositories
{
    /// <summary>
    /// Repository class for performing CRUD operations on User entities in the Firebase database.
    /// Inherits from <see cref="BaseRepository{User}"/> and provides access to User records.
    /// </summary>
    public class UserRepository : BaseRepository<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// Sets the base URL for User records in the Firebase database.
        /// </summary>
        public UserRepository()
            : base("https://crud1-ab551-default-rtdb.firebaseio.com/Users")
        {
        }
    }
}
