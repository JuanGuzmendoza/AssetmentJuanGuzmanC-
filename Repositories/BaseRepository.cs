using System.Text;
using System.Text.Json;
using Hospital.Interfaces;
using Helpers;

namespace Hospital.Repositories
{
    /// <summary>
    /// Base repository class for performing CRUD operations on entities of type <typeparamref name="T"/>.
    /// Provides methods for creating, reading, updating, and deleting records in a remote database.
    /// </summary>
    /// <typeparam name="T">The type of the entity being managed (e.g., Patient, Doctor, Appointment).</typeparam>
    public abstract class BaseRepository<T> : IRepository<T>
    {
        /// <summary>
        /// The <see cref="HttpClient"/> used for sending HTTP requests.
        /// </summary>
        protected readonly HttpClient client;

        /// <summary>
        /// The options used for serializing and deserializing JSON data.
        /// </summary>
        protected readonly JsonSerializerOptions jsonOptions;

        /// <summary>
        /// The base URL of the resource in the remote database.
        /// </summary>
        protected readonly string baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL of the resource in the database.</param>
        protected BaseRepository(string baseUrl)
        {
            this.baseUrl = baseUrl;
            client = new HttpClient();

            jsonOptions = new JsonSerializerOptions
            {
                Converters = { new GuidToStringConverter() },
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false,
            };
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The ID of the created entity.</returns>
        public virtual async Task<string> CreateAsync(T entity)
        {
            try
            {
                string id = Guid.NewGuid().ToString();

                var prop = typeof(T).GetProperty("Id");
                if (prop != null)
                {
                    var value = prop.GetValue(entity);
                    if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                        prop.SetValue(entity, Guid.NewGuid());

                    id = prop.GetValue(entity)?.ToString() ?? id;
                }

                var json = JsonSerializer.Serialize(entity, jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{baseUrl}/{id}.json", content);
                response.EnsureSuccessStatusCode();

                AddToDataStore(id, entity);

                ConsoleUIHelpers.Success($"✅ Record created successfully (ID: {id})");
                return id;
            }
            catch (Exception ex)
            {
                ConsoleUIHelpers.Error($"❌ Error creating record: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Retrieves all records of type <typeparamref name="T"/> from the database.
        /// </summary>
        /// <returns>A dictionary of records indexed by their ID.</returns>
        public virtual async Task<Dictionary<string, T>> GetAllAsync()
        {
            var response = await client.GetAsync($"{baseUrl}.json");
            
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<Dictionary<string, T>>(json, jsonOptions);

            // Si la deserialización fue exitosa, devolver el diccionario
            return result;
        }

        /// <summary>
        /// Retrieves a record by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve.</param>
        /// <returns>The entity with the specified ID.</returns>
        public virtual async Task<T> GetByIdAsync(string id)
        {
            var response = await client.GetAsync($"{baseUrl}/{id}.json");
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, jsonOptions);
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="id">The ID of the entity to update.</param>
        /// <param name="entity">The updated entity.</param>
        public virtual async Task UpdateAsync(string id, T entity)
        {
            try
            {
                var json = JsonSerializer.Serialize(entity, jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PutAsync($"{baseUrl}/{id}.json", content);

                AddToDataStore(id, entity);
                ConsoleUIHelpers.Success($"📝 Record '{id}' updated successfully!");
            }
            catch (Exception ex)
            {
                ConsoleUIHelpers.Error($"❌ Error updating record: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a record by its name (optional, use only if entity has Name property).
        /// </summary>
        /// <param name="name">The name of the record to delete.</param>
        public virtual async Task DeleteAsync(string name)
        {
            try
            {
                var allData = await GetAllAsync();

                var match = allData.FirstOrDefault(p =>
                    typeof(T).GetProperty("Name")?.GetValue(p.Value)?.ToString()
                    ?.Equals(name, StringComparison.OrdinalIgnoreCase) == true);

                if (match.Key == null)
                {
                    ConsoleUIHelpers.Error($"❌ Record '{name}' not found.");
                    await Task.Delay(1200);
                    return;
                }

                await client.DeleteAsync($"{baseUrl}/{match.Key}.json");
                RemoveFromDataStoreById(match.Key);

                ConsoleUIHelpers.Success($"🗑️ Record '{name}' deleted successfully!");
                await Task.Delay(1200);
            }
            catch (Exception ex)
            {
                ConsoleUIHelpers.Error($"❌ Error deleting record: {ex.Message}");
                await Task.Delay(1200);
            }
        }

        /// <summary>
        /// Deletes a record by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the record to delete.</param>
        public virtual async Task DeleteByIdAsync(string id)
        {
            try
            {
                await client.DeleteAsync($"{baseUrl}/{id}.json");
                RemoveFromDataStoreById(id);

                ConsoleUIHelpers.Success($"🗑️ Record '{id}' deleted successfully!");
                await Task.Delay(1200);
            }
            catch (Exception ex)
            {
                ConsoleUIHelpers.Error($"❌ Error deleting record: {ex.Message}");
                await Task.Delay(1200);
            }
        }

        /// <summary>
        /// Removes a record from the in-memory DataStore by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to remove.</param>
        private void RemoveFromDataStoreById(string id)
        {
            try
            {
                string type = typeof(T).Name.ToLower();
                switch (type)
                {
                    case "patient": Data.DataStore.Patients.Remove(id); break;
                    case "user": Data.DataStore.Users.Remove(id); break;
                    case "doctor": Data.DataStore.Doctors.Remove(id); break;
                    case "appointment": Data.DataStore.Appointments.Remove(id); break;
                }
            }
            catch (Exception ex)
            {
                // Puedes loggear aquí la excepción si quieres
                Console.WriteLine($"Warning: Could not remove from DataStore: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a record to the in-memory DataStore.
        /// </summary>
        /// <param name="id">The ID of the record.</param>
        /// <param name="entity">The entity to add to the DataStore.</param>
        private void AddToDataStore(string id, T entity)
        {
            try
            {
                string type = typeof(T).Name.ToLower();
                switch (type)
                {
                    case "patient": Data.DataStore.Patients[id] = entity as dynamic; break;
                    case "user": Data.DataStore.Users[id] = entity as dynamic; break;
                    case "doctor": Data.DataStore.Doctors[id] = entity as dynamic; break;
                    case "appointment": Data.DataStore.Appointments[id] = entity as dynamic; break;
                }
            }
            catch (Exception ex)
            {
                // También puedes loggear aquí si falla
                Console.WriteLine($"Warning: Could not add to DataStore: {ex.Message}");
            }
        }
    }
}
