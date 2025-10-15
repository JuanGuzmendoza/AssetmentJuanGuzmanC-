using Hospital.Data;
using Hospital.Models;

namespace Hospital.Menus
{
    /// <summary>
    /// Provides the login functionality for users and administrators.
    /// It validates the user credentials and routes to the appropriate menu based on the role.
    /// </summary>
    public static class Login
    {
        /// <summary>
        /// Displays the login interface where the user can enter their username and password.
        /// It validates the credentials and routes the user to the correct menu based on their role.
        /// </summary>
        /// <returns>A task representing the asynchronous operation of the login process.</returns>
        public static async Task ShowAsync()
        {
            Console.Title = "🔐 Hospital System - Login";

            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("====================================");
                Console.WriteLine("         🏥 HOSPITAL SYSTEM 🏥        ");
                Console.WriteLine("====================================");
                Console.ResetColor();

                Console.Write("\n👤 Username: ");
                string? username = Console.ReadLine();

                Console.Write("🔒 Password: ");
                string? password = ReadPassword();

                // Validate credentials for admin
                if (username?.Equals("admin", StringComparison.OrdinalIgnoreCase) == true && password == "123")
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n✅ Welcome Admin!");
                    Console.ResetColor();
                    await Task.Delay(1000);
                    Console.Clear();
                    await AdminMenu.ShowAsync();
                    break;
                }

                // Validate credentials from stored users
                if (ValidateCredentials(username, password, out User? user))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n✅ Welcome {user.Name}! Role: {user.Role}");
                    Console.ResetColor();
                    await Task.Delay(1000);
                    Console.Clear();

                    switch (user.Role.ToLower())
                    {
                        case "patient":
                            await PatientMenu.ShowAsync(user);
                            break;
                        case "doctor":
                            await DoctorMenu.ShowAsync(user);
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("⚠️ Unknown role. Access denied.");
                            Console.ResetColor();
                            await Task.Delay(1500);
                            break;
                    }

                    break; // Exit loop after successful login
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n❌ Incorrect username or password.");
                    Console.ResetColor();
                    await Task.Delay(1500);
                }
            }
        }

        /// <summary>
        /// Validates the user's credentials by checking the username and password against the stored users.
        /// </summary>
        /// <param name="username">The username entered by the user.</param>
        /// <param name="password">The password entered by the user.</param>
        /// <param name="user">The user object that matches the credentials, if valid.</param>
        /// <returns>True if the credentials are valid, otherwise false.</returns>
        private static bool ValidateCredentials(string? username, string? password, out User? user)
        {
            user = null;
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            foreach (var u in DataStore.Users.Values)
            {
                if (u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password)
                {
                    user = u;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Reads the password entered by the user in a secure way (characters are hidden as they are typed).
        /// </summary>
        /// <returns>The entered password as a string.</returns>
        private static string ReadPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password[..^1];
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}
