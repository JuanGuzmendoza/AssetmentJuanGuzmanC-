namespace Hospital.Models
{
    using System;

    /// <summary>
    /// Represents an email log entry for tracking email notifications related to appointments.
    /// </summary>
    public class EmailLog
    {
        /// <summary>
        /// Gets or sets the unique identifier of the email log.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the appointment ID that this email is related to.
        /// This is a foreign key representing the appointment associated with the email.
        /// </summary>
        public Guid AppointmentId { get; set; }

        /// <summary>
        /// Gets or sets the recipient email address (typically the patient's email).
        /// </summary>
        public string RecipientEmail { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the email was sent.
        /// Default is set to the current date and time when the email is created.
        /// </summary>
        public DateTime SentDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets the status of the email, indicating whether it was sent or not.
        /// </summary>
        public EmailStatus Status { get; set; } = EmailStatus.NotSent;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailLog"/> class.
        /// </summary>
        public EmailLog() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailLog"/> class with specified appointment ID and recipient email.
        /// </summary>
        /// <param name="appointmentId">The ID of the related appointment.</param>
        /// <param name="recipientEmail">The recipient email address (patient's email).</param>
        public EmailLog(Guid appointmentId, string recipientEmail)
        {
            AppointmentId = appointmentId;
            RecipientEmail = recipientEmail;
        }

        /// <summary>
        /// Displays the information of the email log to the console.
        /// </summary>
        /// <param name="email">The <see cref="EmailLog"/> instance to display.</param>
        public static void ShowInformation(EmailLog email)
        {
            Console.WriteLine($"\n📧 EMAIL LOG");
            Console.WriteLine($"ID             : {email.Id}");
            Console.WriteLine($"Appointment ID : {email.AppointmentId}");
            Console.WriteLine($"To             : {email.RecipientEmail}");
            Console.WriteLine($"Sent Date      : {email.SentDate}");
            Console.WriteLine($"Status         : {email.Status}");
        }
    }

    /// <summary>
    /// Represents the status of the email in the email log.
    /// </summary>
    public enum EmailStatus
    {
        /// <summary>
        /// Indicates the email was successfully sent.
        /// </summary>
        Sent,

        /// <summary>
        /// Indicates the email was not sent.
        /// </summary>
        NotSent
    }
}
