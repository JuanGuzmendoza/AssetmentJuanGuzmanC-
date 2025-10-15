using Hospital.Models;

namespace Hospital.Repositories
{
    /// <summary>
    /// Repository class for performing CRUD operations on Patient entities in the Firebase database.
    /// Inherits from <see cref="BaseRepository{Patient}"/> and provides access to Patient records.
    /// </summary>
    public class PatientRepository : BaseRepository<Patient>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientRepository"/> class.
        /// Sets the base URL for Patient records in the Firebase database.
        /// </summary>
        public PatientRepository()
            : base("https://crud1-ab551-default-rtdb.firebaseio.com/Patients")
        {
        }
    }
}
