using System.Text.Json;
using System.Text.Json.Serialization;

    /// <summary>
    /// A custom JSON converter that converts a <see cref="Guid"/> to and from a <see cref="string"/> representation.
    /// This is particularly useful for serializing and deserializing <see cref="Guid"/> objects as strings in JSON.
    /// </summary>
    public class GuidToStringConverter : JsonConverter<Guid>
    {
        /// <summary>
        /// Reads the JSON string representation of a <see cref="Guid"/> and converts it into a <see cref="Guid"/> object.
        /// </summary>
        /// <param name="reader">The <see cref="Utf8JsonReader"/> that reads the JSON data.</param>
        /// <param name="typeToConvert">The target type to convert to (<see cref="Guid"/> in this case).</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> that might contain settings for serialization.</param>
        /// <returns>A <see cref="Guid"/> object parsed from the JSON string.</returns>
        /// <exception cref="FormatException">Thrown if the JSON string is not in a valid <see cref="Guid"/> format.</exception>
        public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Guid.Parse(reader.GetString());
        }

        /// <summary>
        /// Writes the <see cref="Guid"/> as a JSON string representation.
        /// </summary>
        /// <param name="writer">The <see cref="Utf8JsonWriter"/> to write the JSON data to.</param>
        /// <param name="value">The <see cref="Guid"/> value to serialize into JSON.</param>
        /// <param name="options">The <see cref="JsonSerializerOptions"/> that might contain settings for serialization.</param>
        public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

