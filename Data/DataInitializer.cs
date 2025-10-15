using Hospital.Repositories;

namespace Hospital.Data
{
    /// <summary>
    /// A static class responsible for initializing the data by loading it from various repositories.
    /// It loads patients, doctors, appointments, email logs, and users into memory.
    /// </summary>
    public static class DataInitializer
    {
        /// <summary>
        /// Initializes the data by loading it from repositories into the in-memory data store.
        /// The method also provides feedback to the user via the console, indicating the status of the operation.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method calls the <see cref="PatientRepository"/>, <see cref="DoctorRepository"/>, 
        /// <see cref="AppointmentRepository"/>, <see cref="EmailRepository"/>, and <see cref="UserRepository"/> 
        /// to retrieve data and store it in the <see cref="DataStore"/> static class.
        /// </remarks>
        public static async Task InitializeAsync()
        {
            // Clear the console and show loading message
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⏳ Loading data from database...\n");
            Console.ResetColor();

            // Instantiate repositories for fetching data
            var patientRepo = new PatientRepository();
            var doctorRepo = new DoctorRepository();
            var appointmentRepo = new AppointmentRepository();
            var emailLogRepo = new EmailRepository();
            var userRepo = new UserRepository(); // Optional

            // Load data from repositories into memory
            DataStore.Patients = await patientRepo.GetAllAsync() ?? new();
            DataStore.Doctors = await doctorRepo.GetAllAsync() ?? new();
            DataStore.Appointments = await appointmentRepo.GetAllAsync() ?? new();
            DataStore.EmailLogs = await emailLogRepo.GetAllAsync() ?? new();
            DataStore.Users = await userRepo.GetAllAsync() ?? new(); // Optional

            // Display success message to the user
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✔️ Data loaded successfully.");
            Console.ResetColor();

            // Delay to display the success message before clearing the screen
            await Task.Delay(1200);
            Console.Clear();
        }
    }
}
