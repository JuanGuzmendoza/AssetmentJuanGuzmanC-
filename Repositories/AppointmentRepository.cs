using Hospital.Models;
namespace Hospital.Repositories
{
    /// <summary>
    /// A repository class for managing <see cref="Appointment"/> entities, 
    /// responsible for interacting with the database or API for appointment-related operations.
    /// </summary>
    public class AppointmentRepository : BaseRepository<Appointment>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentRepository"/> class, 
        /// setting the API URL for the appointments collection.
        /// </summary>
        public AppointmentRepository()
            : base("https://crud1-ab551-default-rtdb.firebaseio.com/Appointments")
        {
        }
    }
}
