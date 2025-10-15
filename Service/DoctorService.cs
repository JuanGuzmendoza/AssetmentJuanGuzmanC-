using Hospital.Models;
using Hospital.Repositories;
using Helpers;
using Hospital.Data;

namespace Hospital.Services
{
    /// <summary>
    /// Service responsible for managing doctors in the hospital.
    /// Allows for registering, listing, viewing, updating, and deleting doctors.
    /// </summary>
    public static class DoctorService
    {
        private static readonly DoctorRepository _repository = new();

        /// <summary>
        /// Registers a new doctor in the system.
        /// </summary>
        /// <returns>The unique identifier (GUID) of the newly registered doctor.</returns>
        public static async Task<Guid> RegisterAsync()
        {
            ConsoleUIHelpers.PrintHeader("🩺 REGISTER NEW DOCTOR");

            // Request and validate doctor's details
            string name = Validations.ValidateContent("👤 Enter doctor's name: ");
            int age = Validations.ValidateNumber("🎂 Enter doctor's age: ");
            string address = Validations.ValidateContent("🏠 Enter doctor's address: ");
            string phone = Validations.ValidateContent("📞 Enter doctor's phone: ");
            string email = Validations.ValidateContent("📧 Enter doctor's email: ");
            
            string documentNumber;
            bool documentExists;
            
            // Loop to check if the document number already exists
            while (true)
            {
                documentNumber = Validations.ValidateContent("🆔 Enter doctor's document number: ");
                documentExists = DataStore.Doctors.Values
                    .Any(d => d.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));

                if (!documentExists)
                {
                    break;
                }

                ConsoleUIHelpers.Error("❌ This document number is already registered. Please use a different document number.");
            }

            string specialization = Validations.ValidateContent("🏥 Enter doctor's specialization: ");

            // Create the new Doctor object
            Doctor newDoctor = new(name, age, address, phone, email, documentNumber, specialization);

            // Save the new doctor in Firebase and in memory
            string firebaseId = await _repository.CreateAsync(newDoctor);
            DataStore.Doctors[firebaseId] = newDoctor;

            ConsoleUIHelpers.Success("Doctor registered successfully!");
            Doctor.ShowInformation(newDoctor);
            Console.WriteLine(new string('═', 45));
            ConsoleUIHelpers.Pause();

            return newDoctor.Id;
        }

        /// <summary>
        /// Lists all registered doctors in the system.
        /// </summary>
        public static async Task ListAsync()
        {
            ConsoleUIHelpers.PrintHeader("📋 DOCTOR LIST");

            if (DataStore.Doctors.Count == 0)
            {
                ConsoleUIHelpers.Warning("No doctors found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display information for all doctors
            foreach (var (id, doctor) in DataStore.Doctors)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID: {id}");
                Console.ResetColor();

                Doctor.ShowInformation(doctor);
                Console.WriteLine(new string('-', 45));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Displays the information of a specific doctor based on their name.
        /// </summary>
        /// <remarks>
        /// If the doctor is not found, an error message is displayed.
        /// </remarks>
        public static async Task ShowAsync()
        {
            ConsoleUIHelpers.PrintHeader("🔍 VIEW DOCTOR");

            string name = Validations.ValidateContent("Enter doctor's name: ");
            var match = DataStore.Doctors
                .FirstOrDefault(d => d.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("Doctor not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display the information of the found doctor
            Doctor.ShowInformation(match.Value);
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Updates the details of an existing doctor.
        /// </summary>
        /// <remarks>
        /// If the doctor is not found, an error message is displayed.
        /// The updatable fields include name, age, address, phone, email, document number, and specialization.
        /// </remarks>
        public static async Task UpdateAsync()
        {
            ConsoleUIHelpers.PrintHeader("✏️ UPDATE DOCTOR");

            string name = Validations.ValidateContent("Enter doctor's name to update: ");
            var match = DataStore.Doctors
                .FirstOrDefault(d => d.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("Doctor not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            string firebaseId = match.Key;
            var existing = match.Value;

            Console.WriteLine("\nCurrent Info:");
            Doctor.ShowInformation(existing);
            Console.WriteLine(new string('-', 40));

            // Request new values to update the doctor
            existing.Name = Validations.ValidateContent("👤 Enter new name: ");
            existing.Age = Validations.ValidateNumber("🎂 Enter new age: ");
            existing.Address = Validations.ValidateContent("🏠 Enter new address: ");
            existing.Phone = Validations.ValidateContent("📞 Enter new phone: ");
            existing.Email = Validations.ValidateContent("📧 Enter new email: ");
            existing.DocumentNumber = Validations.ValidateContent("🆔 Enter new document number: ");
            existing.Specialization = Validations.ValidateContent("🏥 Enter new specialization: ");

            // Save the changes in Firebase and memory
            await _repository.UpdateAsync(firebaseId, existing);
            DataStore.Doctors[firebaseId] = existing;

            ConsoleUIHelpers.Success("Doctor updated successfully!");
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Deletes a doctor from the system based on their name.
        /// </summary>
        /// <remarks>
        /// If the doctor is not found, an error message is displayed.
        /// </remarks>
        public static async Task DeleteAsync()
        {
            ConsoleUIHelpers.PrintHeader("🗑️ DELETE DOCTOR");

            string name = Validations.ValidateContent("Enter doctor's name to delete: ");
            var match = DataStore.Doctors
                .FirstOrDefault(d => d.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("Doctor not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            string firebaseId = match.Key;

            // Delete the doctor from Firebase and memory
            await _repository.DeleteAsync(match.Value.Name); // or by ID if necessary
            DataStore.Doctors.Remove(firebaseId);

            ConsoleUIHelpers.Success($"Doctor '{name}' deleted successfully!");
            ConsoleUIHelpers.Pause();
        }
    }
}
