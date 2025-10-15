using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hospital.Models;
using Hospital.Repositories;
using Helpers;
using Hospital.Data;

namespace Hospital.Services
{
    /// <summary>
    /// Service class for managing patient data such as registration, updating, viewing, and deletion.
    /// </summary>
    public static class PatientService
    {
        private static readonly PatientRepository _repository = new();

        /// <summary>
        /// Registers a new patient by prompting for input and validating the data.
        /// </summary>
        /// <returns>The ID of the newly created patient.</returns>
        public static async Task<Guid> RegisterAsync()
        {
            Console.Clear();
            ConsoleUIHelpers.PrintHeader("🧾 REGISTER NEW PATIENT");

            string name = Validations.ValidateContent("👤 Enter patient's name: ");
            int age = Validations.ValidateNumber("🎂 Enter patient's age: ");
            string address = Validations.ValidateContent("🏠 Enter patient's address: ");
            string phone = Validations.ValidateContent("📞 Enter patient's phone: ");
            string email = Validations.ValidateContent("📧 Enter patient's email: ");

            string documentNumber;
            bool documentExists;

            // Loop to verify if the document number already exists
            while (true)
            {
                documentNumber = Validations.ValidateContent("🆔 Enter patient's document number: ");

                // Check if the document already exists in the data store
                documentExists = DataStore.Patients.Values
                    .Any(p => p.DocumentNumber.Equals(documentNumber, StringComparison.OrdinalIgnoreCase));

                if (!documentExists)
                {
                    break;  // Exit the loop if the document is unique
                }

                // If the document already exists, show an error message and continue the loop
                ConsoleUIHelpers.Error("❌ This document number is already registered. Please use a different document number.");
            }

            // Create a new patient if the document is unique
            Patient newPatient = new Patient(name, age, address, phone, email, documentNumber);

            string generatedId = await _repository.CreateAsync(newPatient);

            DataStore.Patients[generatedId] = newPatient;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ PATIENT REGISTERED SUCCESSFULLY!");
            Console.ResetColor();
            Patient.ShowInformation(newPatient);
            Console.WriteLine(new string('═', 45));

            ConsoleUIHelpers.Pause();
            return newPatient.Id;
        }

        /// <summary>
        /// Lists all registered patients.
        /// </summary>
        public static async Task ListAsync()
        {
            Console.Clear();
            ConsoleUIHelpers.PrintHeader("📋 PATIENT LIST");

            if (DataStore.Patients.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("⚠️ No patients found.\n");
                Console.ResetColor();
                ConsoleUIHelpers.Pause();
                return;
            }

            // Display each patient's information
            foreach (var (id, patient) in DataStore.Patients)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"🆔 Firebase ID : {id}");
                Console.ResetColor();

                Patient.ShowInformation(patient);

                Console.WriteLine(new string('-', 45));
            }

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Displays information of a patient by their name.
        /// </summary>
        public static async Task ShowAsync()
        {
            Console.Clear();
            ConsoleUIHelpers.PrintHeader("🔍 VIEW PATIENT");

            string name = Validations.ValidateContent("Enter patient's name: ");
            var patientEntry = DataStore.Patients
                .FirstOrDefault(p => p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (patientEntry.Value == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Patient not found.");
                Console.ResetColor();
                ConsoleUIHelpers.Pause();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Patient.ShowInformation(patientEntry.Value);
            Console.ResetColor();

            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Updates the details of an existing patient.
        /// </summary>
        public static async Task UpdateAsync()
        {
            Console.Clear();
            ConsoleUIHelpers.PrintHeader("✏️ UPDATE PATIENT");

            string name = Validations.ValidateContent("Enter patient's name to update: ");
            var patientEntry = DataStore.Patients
                .FirstOrDefault(p => p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (patientEntry.Value == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Patient not found.");
                Console.ResetColor();
                ConsoleUIHelpers.Pause();
                return;
            }

            string firebaseId = patientEntry.Key;
            var existing = patientEntry.Value;

            Console.WriteLine("\nCurrent Info:");
            Patient.ShowInformation(existing);
            Console.WriteLine(new string('-', 40));

            // Update patient information
            existing.Name = Validations.ValidateContent("👤 Enter new name: ");
            existing.Age = Validations.ValidateNumber("🎂 Enter new age: ");
            existing.Address = Validations.ValidateContent("🏠 Enter new address: ");
            existing.Phone = Validations.ValidateContent("📞 Enter new phone: ");
            existing.Email = Validations.ValidateContent("📧 Enter new email: ");
            existing.DocumentNumber = Validations.ValidateContent("🆔 Enter new document number: ");

            await _repository.UpdateAsync(firebaseId, existing);
            DataStore.Patients[firebaseId] = existing;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✅ Patient updated successfully!");
            Console.ResetColor();
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Deletes a patient by their name.
        /// </summary>
        public static async Task DeleteAsync()
        {
            Console.Clear();
            ConsoleUIHelpers.PrintHeader("🗑️ DELETE PATIENT");

            string name = Validations.ValidateContent("Enter patient's name to delete: ");

            var patientEntry = DataStore.Patients
                .FirstOrDefault(p => p.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (patientEntry.Value == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Patient not found.");
                Console.ResetColor();
                ConsoleUIHelpers.Pause();
                return;
            }

            string firebaseId = patientEntry.Key;

            // Delete the patient from the database and data store
            await _repository.DeleteAsync(name);
            DataStore.Patients.Remove(firebaseId);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n🗑️ Patient '{name}' was deleted successfully!");
            Console.ResetColor();

            Console.WriteLine(new string('═', 45));
            ConsoleUIHelpers.Pause();
        }

        /// <summary>
        /// Shows a patient's profile by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the patient.</param>
        public static async Task ShowByIdAsync(Guid id)
        {
            var match = DataStore.Patients.FirstOrDefault(p => p.Value.Id == id);

            if (match.Value == null)
            {
                ConsoleUIHelpers.Error("Patient not found.");
                ConsoleUIHelpers.Pause();
                return;
            }

            ConsoleUIHelpers.PrintHeader("👤 PATIENT PROFILE");
            Patient.ShowInformation(match.Value);
            ConsoleUIHelpers.Pause();
        }
    }
}
