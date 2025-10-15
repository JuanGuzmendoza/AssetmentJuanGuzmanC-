using Hospital.Models;
using System.Text;
using System.Text.Json;

namespace Hospital.Repositories
{
    /// <summary>
    /// Repository class for performing CRUD operations on Doctor entities in the Firebase database.
    /// This class inherits from <see cref="BaseRepository{Doctor}"/> and allows updates to specific fields of a doctor record.
    /// </summary>
    public class DoctorRepository : BaseRepository<Doctor>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoctorRepository"/> class.
        /// Sets the base URL for Doctor records in the Firebase database.
        /// </summary>
        public DoctorRepository()
            : base("https://crud1-ab551-default-rtdb.firebaseio.com/Doctors")
        {
        }

        /// <summary>
        /// Updates a specific field of a Doctor entity in the Firebase database.
        /// Depending on the field type, it uses either a PATCH or PUT request.
        /// </summary>
        /// <param name="id">The ID of the Doctor record to update.</param>
        /// <param name="field">The field to update (e.g., "Name", "Specialization", etc.).</param>
        /// <param name="value">The new value to set for the field.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="Exception">Throws if the update fails.</exception>
        public async Task UpdateFieldAsync(string id, string field, object value)
        {
            try
            {
                // If the value is a List<Guid>, convert it to List<string>
                if (value is List<Guid> guidList)
                    value = guidList.Select(g => g.ToString()).ToList();

                // Serialize the value to JSON format
                var json = JsonSerializer.Serialize(value, jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Construct the URL for the specific field
                string url = $"{baseUrl}/{id}/{field}.json";

                // Check if the value is an array or collection (excluding string)
                bool isArray = value is System.Collections.IEnumerable && value is not string;

                if (isArray)
                {
                    // Firebase does not support PATCH with arrays – use PUT for arrays
                    Console.WriteLine($"[DEBUG] 🔧 PUT -> {url}");
                    Console.WriteLine($"[DEBUG] 📦 JSON (array): {json}");
                    var response = await client.PutAsync(url, content);
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    // Use PATCH for regular fields
                    Console.WriteLine($"[DEBUG] 🔧 PATCH -> {url}");
                    Console.WriteLine($"[DEBUG] 📦 JSON: {json}");
                    var response = await client.PatchAsync(url, content);
                    response.EnsureSuccessStatusCode();
                }

                Console.WriteLine("[LOG] ✅ Field updated successfully in Firebase.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] ❌ Failed to update field in Firebase: {ex.Message}");
                throw;
            }
        }
    }
}
