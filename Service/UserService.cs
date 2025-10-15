using Hospital.Models;
using Hospital.Repositories;
using Helpers;
using Hospital.Data;

namespace Hospital.Services
{
    /// <summary>
    /// Service class for managing users (e.g., registering, updating, viewing, and listing).
    /// </summary>
    public static class UserService
    {
        private static readonly UserRepository _userRepository = new();

        /// <summary>
        /// Registers a new user by prompting for the user's information and linking them to an entity (Patient/Doctor).
        /// </summary>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        public static async Task RegisterAsync()
        {
            ConsoleUIHelpers.PrintHeader("👤 REGISTER NEW USER");

            string name = Validations.ValidateContent("👉 Enter user's full name: ");
            string username = Validations.ValidateContent("👉 Enter username: ");
            string password = Validations.ValidateContent("👉 Enter password: ");

            // Check if the username already exists
            bool usernameExists = DataStore.Users.Values
                .Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (usernameExists)
            {
                ConsoleUIHelpers.Error("Username already exists. Choose a different one.");
                return;
            }

            // Select the role for the user
            string role = "";
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nSelect the user's role:");
                Console.ResetColor();

                Console.WriteLine("1️⃣  Patient");
                Console.WriteLine("2️⃣  Doctor");
                Console.Write("👉 Option (1 or 2): ");
                string input = Validations.ValidateContent("");

                role = input == "1" ? "Patient" :
                       input == "2" ? "Doctor" : "";

                if (!string.IsNullOrEmpty(role))
                    break;

                ConsoleUIHelpers.Error("❌ Invalid option. Please select 1 or 2.");
            }

            // Register the related entity (Patient or Doctor)
            Guid entityId = role == "Patient"
                ? await PatientService.RegisterAsync()
                : await DoctorService.RegisterAsync();

            // Create the new user
            var newUser = new User(name, username, password, role, entityId);
            string userFirebaseId = await _userRepository.CreateAsync(newUser);
            DataStore.Users[userFirebaseId] = newUser;

            ConsoleUIHelpers.Success("User registered successfully!");
            Console.WriteLine($"\n🆔 Firebase ID: {userFirebaseId}");
            Console.WriteLine($"👤 Name: {newUser.Name}");
            Console.WriteLine($"📛 Username: {newUser.Username}");
            Console.WriteLine($"🔑 Role: {newUser.Role}");
            Console.WriteLine($"🔗 Linked Entity ID: {newUser.EntityId}");

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Lists all registered users.
        /// </summary>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        public static async Task ListAsync()
        {
            ConsoleUIHelpers.PrintHeader("📋 USER LIST");

            if (DataStore.Users == null || DataStore.Users.Count == 0)
            {
                ConsoleUIHelpers.Warning("No users found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display each user's information
            foreach (var entry in DataStore.Users)
            {
                var id = entry.Key;
                var user = entry.Value;

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {id}");
                Console.ResetColor();

                Console.WriteLine($"👤 Name         : {user.Name}");
                Console.WriteLine($"📛 Username     : {user.Username}");
                Console.WriteLine($"🔑 Role         : {user.Role}");
                Console.WriteLine($"🔗 Linked Entity: {user.EntityId}");
                Console.WriteLine(new string('-', 40));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Displays information of a user based on their username or name.
        /// </summary>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        public static async Task ShowAsync()
        {
            ConsoleUIHelpers.PrintHeader("👀 VIEW USER");

            string input = Validations.ValidateContent("Enter user's username or name: ");
            var match = DataStore.Users.FirstOrDefault(u =>
                u.Value.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                u.Value.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("User not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Show the user's information
            User.ShowInformation(match.Value);
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Updates an existing user's information (name, username, password, and role).
        /// </summary>
        /// <returns>Returns a task representing the asynchronous operation.</returns>
        public static async Task UpdateAsync()
        {
            ConsoleUIHelpers.PrintHeader("✏️ UPDATE USER");

            string input = Validations.ValidateContent("Enter user's username or name to update: ");
            var match = DataStore.Users.FirstOrDefault(u =>
                u.Value.Username.Equals(input, StringComparison.OrdinalIgnoreCase) ||
                u.Value.Name.Equals(input, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("User not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            string firebaseId = match.Key;
            var existing = match.Value;

            string newName = Validations.ValidateContent($"Enter new name (current: {existing.Name}): ");
            string newUsername = Validations.ValidateContent($"Enter new username (current: {existing.Username}): ");
            string newPassword = Validations.ValidateContent("Enter new password: ");
            string newRole = Validations.ValidateContent($"Enter new role (Patient / Doctor) (current: {existing.Role}): ");

            // Check if the new username is already taken by another user
            if (!newUsername.Equals(existing.Username, StringComparison.OrdinalIgnoreCase))
            {
                bool usernameExists = DataStore.Users.Values
                    .Any(u => u.Username.Equals(newUsername, StringComparison.OrdinalIgnoreCase));
                if (usernameExists)
                {
                    ConsoleUIHelpers.Error("That username is already taken by another user.");
                    return;
                }
            }

            // Update the user's information
            existing.Name = newName;
            existing.Username = newUsername;
            existing.Password = newPassword;
            existing.Role = newRole;

            await _userRepository.UpdateAsync(firebaseId, existing);
            DataStore.Users[firebaseId] = existing;

            ConsoleUIHelpers.Success("User updated successfully!");
            ConsoleUIHelpers.Pause();
        }
    }
}
