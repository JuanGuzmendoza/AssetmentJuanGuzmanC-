namespace Hospital.Models
{
    using Hospital.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a doctor in the hospital system. The doctor has a specialization and a list of appointments.
    /// </summary>
    public class Doctor : Person, IEntity
    {
        /// <summary>
        /// Gets or sets the specialization of the doctor (e.g., cardiologist, general practitioner).
        /// </summary>
        public string Specialization { get; set; }

        /// <summary>
        /// Gets or sets the list of appointment IDs associated with the doctor.
        /// </summary>
        public List<Guid> AppointmentIds { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class.
        /// </summary>
        public Doctor() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Doctor"/> class with specified values.
        /// </summary>
        /// <param name="name">The name of the doctor.</param>
        /// <param name="age">The age of the doctor.</param>
        /// <param name="address">The address of the doctor.</param>
        /// <param name="phone">The phone number of the doctor.</param>
        /// <param name="email">The email address of the doctor.</param>
        /// <param name="documentNumber">The document number of the doctor (e.g., medical license number).</param>
        /// <param name="specialization">The specialization of the doctor (e.g., cardiology, pediatrics).</param>
        public Doctor(string name, int age, string address, string phone, string email, string documentNumber, string specialization)
            : base(name, age, address, phone, email, documentNumber)
        {
            Specialization = specialization;
            AppointmentIds = new List<Guid>();
        }

        /// <summary>
        /// Displays detailed information about the doctor, including their specialization and associated appointments.
        /// </summary>
        /// <param name="doctor">The doctor whose information is to be displayed.</param>
        public static void ShowInformation(Doctor doctor)
        {
            Console.WriteLine($"\n🩺 DOCTOR INFO");
            Console.WriteLine($"ID           : {doctor.Id}");
            Console.WriteLine($"Name         : {doctor.Name}");
            Console.WriteLine($"Age          : {doctor.Age}");
            Console.WriteLine($"Address      : {doctor.Address}");
            Console.WriteLine($"Phone        : {doctor.Phone}");
            Console.WriteLine($"Email        : {doctor.Email}");
            Console.WriteLine($"Document     : {doctor.DocumentNumber}");
            Console.WriteLine($"Specialization: {doctor.Specialization}");

            if (doctor.AppointmentIds?.Count > 0)
                Console.WriteLine("Appointments: " + string.Join(", ", doctor.AppointmentIds));
            else
                Console.WriteLine("Appointments: None");
        }
    }
}
