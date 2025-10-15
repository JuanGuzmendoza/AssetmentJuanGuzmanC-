namespace Helpers
{
    /// <summary>
    /// A utility class that provides standardized methods for handling console output and user interaction.
    /// These methods are designed to improve the user interface experience in console applications.
    /// </summary>
    public static class ConsoleUIHelpers
    {
        /// <summary>
        /// Prints a styled header at the top of the console with the specified title.
        /// The header is decorated with lines and the title is centered.
        /// </summary>
        /// <param name="title">The title to display in the header.</param>
        /// <remarks>
        /// The header is styled with `═` characters for a clean and consistent appearance.
        /// </remarks>
        public static void PrintHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string('═', 45));
            Console.WriteLine($"     {title}");
            Console.WriteLine(new string('═', 45));
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Pauses the console and waits for the user to press a key before proceeding.
        /// This is typically used after displaying messages, so the user can read them.
        /// </summary>
        /// <remarks>
        /// The console will display a message instructing the user to press any key to continue.
        /// </remarks>
        public static void Pause()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ResetColor();
            Console.ReadKey();
        }

        /// <summary>
        /// Prints a standardized success message to the console with a green color.
        /// Typically used to notify the user that an action was successful.
        /// </summary>
        /// <param name="message">The success message to display.</param>
        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a standardized error message to the console with a red color.
        /// Typically used to notify the user that something went wrong.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {message}");
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a standardized warning message to the console with a yellow color.
        /// Typically used to notify the user about a potential issue or caution.
        /// </summary>
        /// <param name="message">The warning message to display.</param>
        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️ {message}");
            Console.ResetColor();
        }
    }
}
