namespace Helpers
{
    /// <summary>
    /// A static helper class that provides various validation methods for user input.
    /// The methods are used to ensure that input values follow certain criteria, such as
    /// being a valid number, string, or date.
    /// </summary>
    public static class Validations
    {
        /// <summary>
        /// Validates a Yes/No answer from the user.
        /// Prompts the user to input 'y'/'yes' for true and 'n'/'no' for false.
        /// Repeats until valid input is received.
        /// </summary>
        /// <param name="message">The message that is displayed to the user when prompting for input.</param>
        /// <returns>
        /// <c>true</c> if the user enters 'y' or 'yes', <c>false</c> if the user enters 'n' or 'no'.
        /// </returns>
        public static bool ValidateYesNo(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (input == "y" || input == "yes")
                    return true;
                else if (input == "n" || input == "no")
                    return false;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Please enter only 'y' or 'n'.");
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// Validates that the user input is not empty or whitespace.
        /// Repeats until a valid non-empty string is provided.
        /// </summary>
        /// <param name="message">The message that is displayed to the user when prompting for input.</param>
        /// <returns>A valid string that is not empty or whitespace.</returns>
        public static string ValidateContent(string message)
        {
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("⚠️ Invalid input. Please do not leave the field empty.");
                }
                else
                {
                    return input;
                }
            }
        }

        /// <summary>
        /// Validates that the user input is a valid integer number.
        /// Repeats until a valid integer is provided.
        /// </summary>
        /// <param name="message">The message that is displayed to the user when prompting for input.</param>
        /// <returns>A valid integer number.</returns>
        public static int ValidateNumber(string message)
        {
            while (true)
            {
                int number;
                Console.Write(message);
                string? input = Console.ReadLine();
                if (int.TryParse(input, out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter an integer number.");
                }
            }
        }

        /// <summary>
        /// Validates that the user input is a valid floating-point number (double).
        /// Repeats until a valid floating-point number is provided.
        /// </summary>
        /// <param name="message">The message that is displayed to the user when prompting for input.</param>
        /// <returns>A valid floating-point number.</returns>
        public static double ValidateDouble(string message)
        {
            while (true)
            {
                double number;
                Console.Write(message);
                string? input = Console.ReadLine();
                if (double.TryParse(input, out number))
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("⚠️ Invalid input. Please enter a floating-point number.");
                }
            }
        }

        /// <summary>
        /// Validates that the user input is a valid date and time in the format 'yyyy-MM-dd HH:mm'.
        /// Repeats until a valid date and time string is provided.
        /// </summary>
        /// <param name="message">The message that is displayed to the user when prompting for input.</param>
        /// <returns>A valid <see cref="DateTime"/> object representing the entered date and time.</returns>
        public static DateTime ValidateAppointmentDate(string message)
        {
            DateTime appointmentDate;
            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                // If the date is not valid, prompt the user to enter again
                if (!DateTime.TryParseExact(input, "yyyy-MM-dd HH:mm", null, 
                    System.Globalization.DateTimeStyles.None, out appointmentDate))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("⚠️ Invalid date/time format. Please use the format 'yyyy-MM-dd HH:mm'.");
                    Console.ResetColor();
                }
                else
                {
                    return appointmentDate;
                }
            }
        }
    }
}
