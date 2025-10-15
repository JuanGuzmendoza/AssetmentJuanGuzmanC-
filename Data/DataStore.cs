using System.Collections.Generic;
using Hospital.Models;

namespace Hospital.Data
{
    /// <summary>
    /// A static class that acts as an in-memory data store for various hospital entities.
    /// It simulates a database by holding collections of patients, doctors, appointments, email logs, and users.
    /// </summary>
    public static class DataStore
    {
        /// <summary>
        /// A dictionary holding the patients in the system.
        /// The key is a string identifier (e.g., Patient ID) and the value is the <see cref="Patient"/> object.
        /// </summary>
        public static Dictionary<string, Patient> Patients { get; set; } = new();

        /// <summary>
        /// A dictionary holding the doctors in the system.
        /// The key is a string identifier (e.g., Doctor ID) and the value is the <see cref="Doctor"/> object.
        /// </summary>
        public static Dictionary<string, Doctor> Doctors { get; set; } = new();

        /// <summary>
        /// A dictionary holding the appointments in the system.
        /// The key is a string identifier (e.g., Appointment ID) and the value is the <see cref="Appointment"/> object.
        /// </summary>
        public static Dictionary<string, Appointment> Appointments { get; set; } = new();

        /// <summary>
        /// A dictionary holding the email logs in the system.
        /// The key is a string identifier (e.g., Email Log ID) and the value is the <see cref="EmailLog"/> object.
        /// </summary>
        public static Dictionary<string, EmailLog> EmailLogs { get; set; } = new();

        /// <summary>
        /// A dictionary holding the users in the system.
        /// This is optional and could be used for implementing user authentication or authorization.
        /// The key is a string identifier (e.g., User ID) and the value is the <see cref="User"/> object.
        /// </summary>
        public static Dictionary<string, User> Users { get; set; } = new();

        /// <summary>
        /// Clears all data in the store.
        /// This method is useful when resetting the system or logging out users.
        /// </summary>
        /// <remarks>
        /// This method clears all the dictionaries: <see cref="Patients"/>, <see cref="Doctors"/>, 
        /// <see cref="Appointments"/>, <see cref="EmailLogs"/>, and <see cref="Users"/>.
        /// </remarks>
        public static void Clear()
        {
            Patients.Clear();
            Doctors.Clear();
            Appointments.Clear();
            EmailLogs.Clear();
            Users.Clear();
        }
    }
}
