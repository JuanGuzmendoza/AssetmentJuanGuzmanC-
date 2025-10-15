using Hospital.Models;
using Hospital.Services;

namespace Hospital.Menus
{
    /// <summary>
    /// Represents the menu for a doctor, providing options to view their appointments.
    /// </summary>
    public static class DoctorMenu
    {
        /// <summary>
        /// Displays the doctor menu, allowing the doctor to view their appointments.
        /// </summary>
        /// <param name="user">The authenticated user, which is a doctor in this case.</param>
        /// <returns>A task that represents the asynchronous operation of showing the menu and performing actions based on user input.</returns>
        public static async Task ShowAsync(User user)
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine($"🩺 Welcome Dr. {user.Name}, Doctor Menu");
                Console.WriteLine("==========================================");
                Console.WriteLine("1️⃣  View my appointments");
                Console.WriteLine("0️⃣  Exit");
                Console.WriteLine("==========================================");
                Console.Write("Choose an option: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        // Displays all appointments assigned to the doctor
                        await AppointmentService.ListByDoctorAsync(user.EntityId);
                        break;
                    case "0":
                        // Exits from the menu
                        exit = true;
                        break;
                    default:
                        // Handles invalid input
                        Console.WriteLine("⚠️ Invalid option. Try again.");
                        await Task.Delay(1000);
                        break;
                }
            }
        }
    }
}
