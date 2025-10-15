using Hospital.Models;
using Hospital.Repositories;
using Hospital.Data;
using Helpers;

namespace Hospital.Services
{
    /// <summary>
    /// Service class that handles operations related to medical appointments.
    /// This includes creating, listing, and canceling appointments, as well as selecting doctors based on patient symptoms.
    /// </summary>
    public static class AppointmentService
    {
        private static readonly AppointmentRepository _appointmentRepo = new();
        private static readonly DoctorRepository _doctorRepo = new();
        private static readonly EmailRepository _emailLogRepo = new();

        /// <summary>
        /// Creates a new medical appointment for a patient.
        /// The user provides the symptoms, selects a doctor based on AI suggestions, and schedules an appointment.
        /// </summary>
        /// <param name="patientId">The ID of the patient requesting the appointment.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task CreateAsync(Guid patientId)
        {
            Console.Clear();
            Console.WriteLine("🩺 Describe your health issue:");
            string description = Console.ReadLine() ?? "";

            var doctors = await _doctorRepo.GetAllAsync();

            if (doctors.Count == 0) return;

            // Prompt for a valid appointment date
            DateTime appointmentDate = Validations.ValidateAppointmentDate("\n📆 Enter desired appointment date and time (format: yyyy-MM-dd HH:mm):");

            // Use GeminiService to select the most suitable doctor based on symptoms
            var suggested = await GeminiService.SelectDoctorAsync(description, doctors);

            if (suggested == null || suggested.SelectedDoctorId == Guid.Empty)
            {
                ConsoleUIHelpers.Error("❌ No doctor could be assigned by AI.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Fetch the selected doctor
            var selectedDoctor = doctors.Values.FirstOrDefault(d => d.Id == suggested.SelectedDoctorId);

            // Check if the doctor is available at the requested time
            var doctorAppointments = DataStore.Appointments.Values
                .Where(a => a.DoctorId == selectedDoctor.Id)
                .ToList();

            bool isAvailable = !doctorAppointments.Any(a =>
                Math.Abs((a.AppointmentDate - appointmentDate).TotalMinutes) < 60
            );

            if (!isAvailable)
            {
                ConsoleUIHelpers.Error($"❌ Doctor {selectedDoctor.Name} is NOT available at that date/time.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Create a new appointment
            var appointment = new Appointment(patientId, selectedDoctor.Id, appointmentDate);
            await _appointmentRepo.CreateAsync(appointment);

            // Update the doctor's appointment list
            selectedDoctor.AppointmentIds ??= new List<Guid>();
            selectedDoctor.AppointmentIds.Add(appointment.Id);
            await _doctorRepo.UpdateFieldAsync(selectedDoctor.Id.ToString(), "appointmentIds", selectedDoctor.AppointmentIds);

            // Store updated doctor data
            DataStore.Doctors[selectedDoctor.Id.ToString()] = selectedDoctor;

            // Send confirmation email to the patient
            string patientEmail = DataStore.Patients[patientId.ToString()].Email;
            await EmailService.SendAppointmentEmailAsync(appointment, patientEmail, selectedDoctor, EmailAction.Confirmation);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Appointment successfully created and email sent!");
            Console.ResetColor();

            // Display appointment information
            Appointment.ShowInformation(appointment);
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Lists all appointments for a given patient.
        /// Displays a list of appointments with detailed information.
        /// </summary>
        /// <param name="patientId">The ID of the patient whose appointments are to be listed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ListByPatientAsync(Guid patientId)
        {
            ConsoleUIHelpers.PrintHeader("📅 My Appointments");

            // Fetch appointments for the patient
            var appointments = DataStore.Appointments.Values
                .Where(a => a.PatientId == patientId)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            if (!appointments.Any())
            {
                ConsoleUIHelpers.Warning("No appointments found for this patient.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display each appointment
            foreach (var appt in appointments)
            {
                Appointment.ShowInformation(appt);
                Console.WriteLine(new string('-', 40));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Lists all appointments for a given doctor.
        /// Displays a list of appointments with detailed information.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor whose appointments are to be listed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ListByDoctorAsync(Guid doctorId)
        {
            ConsoleUIHelpers.PrintHeader("📅 Doctor's Appointments");

            // Fetch appointments for the doctor
            var appointments = DataStore.Appointments.Values
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            if (!appointments.Any())
            {
                ConsoleUIHelpers.Warning("No appointments found for this doctor.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display each appointment
            foreach (var appt in appointments)
            {
                Appointment.ShowInformation(appt);
                Console.WriteLine(new string('-', 40));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Lists all appointments in the system.
        /// Displays all appointments ordered by date and time.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ListAllAppointmentsAsync()
        {
            ConsoleUIHelpers.PrintHeader("📅 All Appointments");

            // Fetch all appointments from the system
            var appointments = DataStore.Appointments.Values
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            if (!appointments.Any())
            {
                ConsoleUIHelpers.Warning("No appointments found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display each appointment
            foreach (var appt in appointments)
            {
                Appointment.ShowInformation(appt);
                Console.WriteLine(new string('-', 40));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Cancels an appointment by selecting it from a list of scheduled appointments.
        /// The patient and doctor are notified via email.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task CancelAsync()
        {
            Console.Clear();
            Console.WriteLine("📅 Cancel Appointment");

            // Fetch all scheduled appointments
            var appointments = DataStore.Appointments.Values
                .Where(a => a.Status == AppointmentStatus.Scheduled)
                .OrderBy(a => a.AppointmentDate)
                .ToList();

            if (appointments.Count == 0)
            {
                ConsoleUIHelpers.Warning("❌ No scheduled appointments found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display a list of appointments to choose from
            Console.WriteLine("Please select an appointment to cancel:");
            for (int i = 0; i < appointments.Count; i++)
            {
                var appt = appointments[i];
                Console.WriteLine($"\n[{i + 1}]");
                Console.WriteLine($"📅 Appointment ID: {appt.Id}");
                Console.WriteLine($"🩺 Doctor ID: {appt.DoctorId} - Date: {appt.AppointmentDate:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"👤 Patient ID: {appt.PatientId}");
                Console.WriteLine($"🔔 Status: {appt.Status}");
                Console.WriteLine(new string('-', 40)); // Separator
            }

            int choice = Validations.ValidateNumber("\n👉 Select the number of the appointment to cancel: ");

            // Cancel the selected appointment
            var selectedAppointment = appointments[choice - 1];
            selectedAppointment.Status = AppointmentStatus.Canceled;

            // Update the appointment in the repository
            await _appointmentRepo.UpdateAsync(selectedAppointment.Id.ToString(), selectedAppointment);

            // Send cancellation email to patient
            var patientEmail = DataStore.Patients[selectedAppointment.PatientId.ToString()].Email;
            await EmailService.SendAppointmentEmailAsync(selectedAppointment, patientEmail, DataStore.Doctors[selectedAppointment.DoctorId.ToString()], EmailAction.Cancellation);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Appointment has been successfully canceled and the patient notified!");
            Console.ResetColor();
            ConsoleUIHelpers.Pause();
        }
    }
}
