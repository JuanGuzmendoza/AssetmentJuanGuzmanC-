using Hospital.Models;

namespace Hospital.Repositories
{
    /// <summary>
    /// Repository class for performing CRUD operations on EmailLog entities in the Firebase database.
    /// Inherits from <see cref="BaseRepository{EmailLog}"/> and provides access to EmailLog records.
    /// </summary>
    public class EmailRepository : BaseRepository<EmailLog>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailRepository"/> class.
        /// Sets the base URL for EmailLog records in the Firebase database.
        /// </summary>
        public EmailRepository()
            : base("https://crud1-ab551-default-rtdb.firebaseio.com/Emails")
        {
        }
    }
}
