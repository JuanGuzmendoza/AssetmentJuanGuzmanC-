namespace Hospital.Models
{
    /// <summary>
    /// Represents a person (either a patient or doctor) in the hospital system.
    /// This class contains common properties shared by all persons in the system.
    /// </summary>
    public abstract class Person
    {
        /// <summary>
        /// Gets or sets the unique identifier for the person.
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the age of the person.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the address of the person.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the phone number of the person.
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets the email address of the person.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the document number (e.g., ID or social security number) of the person.
        /// This acts as a unique identifier for the person.
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        protected Person() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class with the specified details.
        /// </summary>
        /// <param name="name">The name of the person.</param>
        /// <param name="age">The age of the person.</param>
        /// <param name="address">The address of the person.</param>
        /// <param name="phone">The phone number of the person.</param>
        /// <param name="email">The email address of the person.</param>
        /// <param name="documentNumber">The document number (e.g., ID or social security number) of the person.</param>
        protected Person(string name, int age, string address, string phone, string email, string documentNumber)
        {
            Name = name;
            Age = age;
            Address = address;
            Phone = phone;
            Email = email;
            DocumentNumber = documentNumber;
        }
    }
}
