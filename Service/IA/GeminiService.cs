using System.Text;
using System.Text.Json;
using Hospital.Models;
using Hospital.Data;

namespace Hospital.Services
{
    /// <summary>
    /// Service class to interact with the Gemini API for generating content and making decisions based on hospital data.
    /// </summary>
    public static class GeminiService
    {
        // API Key and Client for Gemini API
        private static readonly string _apiKey = "AIzaSyCNZZPfPm3AgVYQyb_CNFWGmzuCO532DM8";  // API Key for authentication
        private static readonly HttpClient _client = new();  // HTTP client for API requests
        private static readonly string _baseUrl =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";  // API URL

        /// <summary>
        /// Method to select the most suitable doctor based on the symptoms provided by the user.
        /// Sends a request to the Gemini API to analyze the symptoms and recommend a doctor.
        /// </summary>
        /// <param name="symptoms">A string describing the patient's symptoms.</param>
        /// <param name="doctors">A dictionary of available doctors.</param>
        /// <returns>A <see cref="SelectedDoctorResult"/> containing the selected doctor's ID and the reason for selection.</returns>
        public static async Task<SelectedDoctorResult> SelectDoctorAsync(string symptoms, Dictionary<string, Doctor> doctors)
        {
            // Creates a list of doctors for the API prompt
            var doctorList = string.Join(", ", doctors.Values.Select(d => $"{d.Id}:{d.Name}-{d.Specialization}"));

            // Building the prompt for the Gemini API
            var prompt = $@"
You have the following doctors: {doctorList}.
Based on the patient's symptoms: ""{symptoms}"",
choose the most suitable doctor and respond ONLY in EXACT JSON format like this:
{{
  ""selectedDoctorId"": ""doctor_id_here"",
  ""reason"": ""brief explanation of why you selected this doctor""
}}";

            // Request body to send to Gemini API
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            // Serialize the request body to JSON
            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Make the API request
            var response = await _client.PostAsync($"{_baseUrl}?key={_apiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
            }

            // Parse the response
            var jsonResponse = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonResponse);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            text = text?
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            SelectedDoctorResult? result = null;

            try
            {
                result = JsonSerializer.Deserialize<SelectedDoctorResult>(text, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                // Fallback in case of a bad format
            }

            return result ?? new SelectedDoctorResult
            {
                SelectedDoctorId = Guid.Empty,
                Reason = "The doctor could not be determined."
            };
        }

        /// <summary>
        /// Method to ask general clinic-related questions using stored data from DataStore.
        /// Sends the question along with the hospital's data to the Gemini API for an answer.
        /// </summary>
        /// <param name="question">The question to ask the Gemini model.</param>
        /// <returns>A string containing the response from Gemini to the question.</returns>
        public static async Task<string> AskClinicQuestionAsync(string question)
        {
            // Collecting all relevant data directly from DataStore
            var patients = DataStore.Patients.Values.ToList();  // Full patient data
            var doctors = DataStore.Doctors.Values.ToList();  // Full doctor data
            var appointments = DataStore.Appointments.Values.ToList();  // Full appointment data
            var emailLogs = DataStore.EmailLogs.Values.ToList();  // Full email log data

            // Constructing the prompt to send to Gemini with the full objects
            var prompt = $@"
You are an assistant helping with a hospital. Here is some important data:
1. Patients: {JsonSerializer.Serialize(patients)}.
2. Doctors: {JsonSerializer.Serialize(doctors)}.
3. Appointments: {JsonSerializer.Serialize(appointments)}.
4. Email Logs: {JsonSerializer.Serialize(emailLogs)}.

Answer the following question based on this data: '{question}'";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            // Serialize the request body to JSON
            var jsonBody = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            // Make the API request
            var response = await _client.PostAsync($"{_baseUrl}?key={_apiKey}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error: {response.StatusCode} - {error}");
            }

            // Parse the response
            var jsonResponse = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(jsonResponse);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text?.Trim() ?? "Sorry, I couldn't answer that question.";
        }
    }

    /// <summary>
    /// Result model used to store the selected doctor and reason for the selection.
    /// </summary>
    public class SelectedDoctorResult
    {
        /// <summary>
        /// Gets or sets the ID of the selected doctor.
        /// </summary>
        public Guid SelectedDoctorId { get; set; }

        /// <summary>
        /// Gets or sets the reason for selecting this doctor.
        /// </summary>
        public string Reason { get; set; } = "";
    }
}
