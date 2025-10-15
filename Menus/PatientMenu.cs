using Hospital.Models;
using Hospital.Services;
using Hospital.Data;

namespace Hospital.Menus
{
    /// <summary>
    /// Represents the menu for a patient, providing options to interact with appointments and view their profile.
    /// </summary>
    public static class PatientMenu
    {
        /// <summary>
        /// Displays the patient menu, providing options to make appointments, view appointments, or view the patient's profile.
        /// </summary>
        /// <param name="user">The authenticated user, which is a patient in this case.</param>
        /// <returns>A task that represents the asynchronous operation of showing the menu and performing actions based on user input.</returns>
        public static async Task ShowAsync(User user)
        {
            bool exit = false;

            while (!exit)
            {
                // Load the latest data from the database or in-memory store
                await DataInitializer.InitializeAsync(); 

                Console.Clear();
                Console.WriteLine($"👋 Welcome, {user.Name}!");
                Console.WriteLine("========= PATIENT MENU =========");
                Console.WriteLine("1️⃣  Make an appointment");
                Console.WriteLine("2️⃣  View my appointments");
                Console.WriteLine("3️⃣  View my profile");
                Console.WriteLine("0️⃣  Back to main menu");
                Console.WriteLine("=================================\n");
                Console.Write("Select an option: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        // Allows the patient to make an appointment
                        await AppointmentService.CreateAsync(user.EntityId); 
                        break;

                    case "2":
                        // Allows the patient to view their appointments
                        await AppointmentService.ListByPatientAsync(user.EntityId);
                        break;

                    case "3":
                        // Displays the patient's profile
                        await PatientService.ShowByIdAsync(user.EntityId);
                        break;

                    case "0":
                        // Exits to the main menu
                        exit = true;
                        Console.WriteLine("👋 Returning to main menu...");
                        await Task.Delay(800);
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
