namespace Hospital.Models
{
    /// <summary>
    /// Represents a user in the hospital system, including their credentials and role.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username of the user. Used for login.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the user. Used for login validation.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the role of the user (e.g., "admin", "doctor", "patient").
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entity linked to the user (e.g., patient ID, doctor ID).
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class with the specified details.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="role">The role of the user (e.g., "admin", "doctor", "patient").</param>
        /// <param name="entityId">The ID of the entity linked to the user (e.g., patient ID, doctor ID).</param>
        public User(string name, string username, string password, string role, Guid entityId)
        {
            Name = name;
            Username = username;
            Password = password;
            Role = role;
            EntityId = entityId;
        }

        /// <summary>
        /// Displays detailed information about the user.
        /// </summary>
        /// <param name="user">The user whose information is to be displayed.</param>
        public static void ShowInformation(User user)
        {
            Console.WriteLine($"\n👤 User ID: {user.Id}");
            Console.WriteLine($"   Name: {user.Name}");
            Console.WriteLine($"   Username: {user.Username}");
            Console.WriteLine($"   Role: {user.Role}");
            Console.WriteLine($"   Linked Entity ID: {user.EntityId}");
        }
    }
}
