using Hospital.Services;
using Helpers;

namespace Hospital.Menus
{
    /// <summary>
    /// Provides the main administrator menu for managing hospital-related entities, including patients, doctors, appointments, users, and email logs.
    /// The menu offers various options and navigates to different management functionalities.
    /// </summary>
    public static class AdminMenu
    {
        /// <summary>
        /// Displays the administrator menu and handles navigation to various management sections.
        /// The menu includes options to manage patients, doctors, appointments, users, email logs, and more.
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying the menu and handling user input.</returns>
        public static async Task ShowAsync()
        {
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("====================================");
                Console.WriteLine("        ⚙️  ADMINISTRATOR MENU        ");
                Console.WriteLine("====================================");
                Console.ResetColor();

                Console.WriteLine("\nSelect an option:");
                Console.WriteLine("1️⃣  Manage Patients");
                Console.WriteLine("2️⃣  Manage Doctors");
                Console.WriteLine("3️⃣  Manage Appointments");
                Console.WriteLine("4️⃣  Manage Users");
                Console.WriteLine("5️⃣  View Email Logs"); 
                Console.WriteLine("6️⃣  Hospital Analytics with AI"); 
                Console.WriteLine("0️⃣  Exit");
                Console.Write("\n👉 Option: ");

                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await ShowPatientMenuAsync();
                        break;
                    case "2":
                        await ShowDoctorMenuAsync();
                        break;
                    case "3":
                        await ShowAppointmentMenuAsync(); // Uncomment once appointment menu is implemented
                        break;
                    case "4":
                        await ShowUserMenuAsync();
                        break;
                    case "5":
                        await ShowEmailLogsAsync(); // New case for Email Logs
                        break;
                    case "6":
                        await AskClinicQuestionAsync();  // New option for AI
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        ShowError("Invalid option. Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Allows the administrator to ask general questions about the clinic using AI (Gemini service).
        /// </summary>
        /// <returns>A task representing the asynchronous operation of asking a clinic-related question and receiving an answer.</returns>
        private static async Task AskClinicQuestionAsync()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("===== 🏥 ASK GENERAL CLINIC QUESTIONS =====");
            Console.ResetColor();

            Console.WriteLine("Please enter your question about the clinic:");
            string question = Console.ReadLine() ?? string.Empty;

            // Consult the Gemini service for the question
            string answer = await GeminiService.AskClinicQuestionAsync(question);

            Console.WriteLine("\nAI Answer:");
            Console.WriteLine(answer);

            ConsoleUIHelpers.Pause();
        }

        // 👥 PATIENT MENU
        /// <summary>
        /// Displays the menu for managing patients (list, edit, delete).
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying the patient management menu.</returns>
        private static async Task ShowPatientMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===== 👥 PATIENT MANAGEMENT =====");
                Console.ResetColor();

                Console.WriteLine("1️⃣  List patients");
                Console.WriteLine("2️⃣  Edit patient");
                Console.WriteLine("3️⃣  Delete patient");
                Console.WriteLine("0️⃣  Back");
                Console.Write("\n👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await PatientService.ListAsync(); break;
                    case "2": await PatientService.UpdateAsync(); break;
                    case "3": await PatientService.DeleteAsync(); break;
                    case "0":
                        back = true;
                        Console.Clear();
                        break;
                    default: ShowError("Invalid option."); break;
                }
            }
        }

        // 📅 APPOINTMENT MENU
        /// <summary>
        /// Displays the menu for managing appointments (list, cancel, mark as attended).
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying the appointment management menu.</returns>
        private static async Task ShowAppointmentMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===== 📅 APPOINTMENT MANAGEMENT =====");
                Console.ResetColor();

                Console.WriteLine("1️⃣  List appointments");
                Console.WriteLine("2️⃣  Cancel appointment");
                Console.WriteLine("3️⃣  Mark as attended");
                Console.WriteLine("0️⃣  Back");
                Console.Write("\n👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await AppointmentService.ListAllAppointmentsAsync(); break;
                    case "2": await AppointmentService.CancelAsync(); break;
                    case "0":
                        back = true;
                        Console.Clear();
                        break;
                    default: ShowError("Invalid option."); break;
                }
            }
        }

        // 🩺 DOCTOR MENU
        /// <summary>
        /// Displays the menu for managing doctors (list, edit, delete).
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying the doctor management menu.</returns>
        private static async Task ShowDoctorMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===== 🩺 DOCTOR MANAGEMENT =====");
                Console.ResetColor();

                Console.WriteLine("1️⃣  List doctors");
                Console.WriteLine("2️⃣  Edit doctor");
                Console.WriteLine("3️⃣  Delete doctor");
                Console.WriteLine("0️⃣  Back");
                Console.Write("\n👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await DoctorService.ListAsync(); break;
                    case "2": await DoctorService.UpdateAsync(); break;
                    case "3": await DoctorService.DeleteAsync(); break;
                    case "0":
                        back = true;
                        Console.Clear();
                        break;
                    default: ShowError("Invalid option."); break;
                }
            }
        }

        // 👤 USER MENU
        /// <summary>
        /// Displays the menu for managing users (list, add, edit).
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying the user management menu.</returns>
        private static async Task ShowUserMenuAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===== 👤 USER MANAGEMENT =====");
                Console.ResetColor();

                Console.WriteLine("1️⃣  List users");
                Console.WriteLine("2️⃣  Add user");
                Console.WriteLine("3️⃣  Edit user");
                Console.WriteLine("0️⃣  Back");
                Console.Write("\n👉 Option: ");
                string? option = Console.ReadLine();

                switch (option)
                {
                    case "1": await UserService.ListAsync(); break;
                    case "2": await UserService.RegisterAsync(); break;
                    case "3": await UserService.UpdateAsync(); break;
                    case "0":
                        back = true;
                        Console.Clear();
                        break;
                    default: ShowError("Invalid option."); break;
                }
            }
        }

        // 📨 EMAIL LOGS MENU
        /// <summary>
        /// Displays the menu to view all email logs.
        /// </summary>
        /// <returns>A task representing the asynchronous operation of displaying email logs.</returns>
        private static async Task ShowEmailLogsAsync()
        {
            // Call the method to show all email logs
            await EmailService.ViewAllEmailHistoryAsync();
        }

        // ⚠️ ERROR MESSAGE
        /// <summary>
        /// Displays an error message to the console.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n❌ {message}");
            Console.ResetColor();
            Task.Delay(1200).Wait();
            Console.Clear();
        }
    }
}
