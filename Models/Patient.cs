namespace Hospital.Models
{
    using Hospital.Interfaces;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a patient in the hospital system.
    /// Inherits from the <see cref="Person"/> class and implements <see cref="IEntity"/>.
    /// </summary>
    public class Patient : Person, IEntity
    {
        /// <summary>
        /// Gets or sets the list of appointment IDs associated with the patient.
        /// This represents all the appointments the patient has scheduled.
        /// </summary>
        public List<Guid> AppointmentIds { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class.
        /// </summary>
        public Patient() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Patient"/> class with the specified details.
        /// </summary>
        /// <param name="name">The name of the patient.</param>
        /// <param name="age">The age of the patient.</param>
        /// <param name="address">The address of the patient.</param>
        /// <param name="phone">The phone number of the patient.</param>
        /// <param name="email">The email address of the patient.</param>
        /// <param name="documentNumber">The document number (e.g., ID or social security number) of the patient.</param>
        public Patient(string name, int age, string address, string phone, string email, string documentNumber)
            : base(name, age, address, phone, email, documentNumber)
        {
            AppointmentIds = new List<Guid>();
        }

        /// <summary>
        /// Displays the information of the patient to the console.
        /// </summary>
        /// <param name="patient">The <see cref="Patient"/> instance whose information is to be displayed.</param>
        public static void ShowInformation(Patient patient)
        {
            Console.WriteLine($"\n👤 PATIENT INFO");
            Console.WriteLine($"ID      : {patient.Id}");
            Console.WriteLine($"Name    : {patient.Name}");
            Console.WriteLine($"Age     : {patient.Age}");
            Console.WriteLine($"Address : {patient.Address}");
            Console.WriteLine($"Phone   : {patient.Phone}");
            Console.WriteLine($"Email   : {patient.Email}");
            Console.WriteLine($"Document: {patient.DocumentNumber}");
        }
    }
}
