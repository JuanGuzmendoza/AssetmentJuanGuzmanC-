namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Represents an appointment in the hospital system, linking a patient to a doctor for a specific time.
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// Gets or sets the unique identifier of the appointment.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the unique identifier of the patient associated with the appointment.
        /// </summary>
        public Guid PatientId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the doctor associated with the appointment.
        /// </summary>
        public Guid DoctorId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the appointment is scheduled.
        /// </summary>
        public DateTime AppointmentDate { get; set; }

        /// <summary>
        /// Gets or sets the status of the appointment (e.g., scheduled, attended, canceled).
        /// </summary>
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class with default values.
        /// </summary>
        public Appointment() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Appointment"/> class with specified values.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient.</param>
        /// <param name="doctorId">The unique identifier of the doctor.</param>
        /// <param name="appointmentDate">The date and time of the appointment.</param>
        public Appointment(Guid patientId, Guid doctorId, DateTime appointmentDate)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            AppointmentDate = appointmentDate;
            Status = AppointmentStatus.Scheduled;
        }

        /// <summary>
        /// Displays detailed information about the appointment.
        /// </summary>
        /// <param name="appt">The appointment to display information for.</param>
        public static void ShowInformation(Appointment appt)
        {
            Console.WriteLine($"\n📅 APPOINTMENT INFO");
            Console.WriteLine($"ID         : {appt.Id}");
            Console.WriteLine($"Patient ID : {appt.PatientId}");
            Console.WriteLine($"Doctor ID  : {appt.DoctorId}");
            Console.WriteLine($"Date       : {appt.AppointmentDate}");
            Console.WriteLine($"Status     : {appt.Status}");
        }
    }

    /// <summary>
    /// Enum representing the status of an appointment.
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// Indicates the appointment is scheduled.
        /// </summary>
        Scheduled,

        /// <summary>
        /// Indicates the appointment was attended by the patient.
        /// </summary>
        Attended,

        /// <summary>
        /// Indicates the appointment was canceled.
        /// </summary>
        Canceled
    }
}
