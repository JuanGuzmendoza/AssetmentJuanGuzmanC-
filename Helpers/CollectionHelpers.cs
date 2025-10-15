using Hospital.Data;
using Hospital.Models;

namespace Helpers
{
    /// <summary>
    /// A helper class that contains methods for interacting with Firebase data stored in dictionaries.
    /// </summary>
    public static class FirebaseFinder
    {
        /// <summary>
        /// Retrieves the Firebase ID (key) of an object based on the provided name.
        /// This method searches through the given dictionary and matches the <see cref="dynamic.Name"/> 
        /// property of each object to the provided name.
        /// </summary>
        /// <param name="name">The name of the object to search for in the dictionary.</param>
        /// <param name="dictionary">The dictionary where the data is stored. 
        /// The dictionary key is the Firebase ID, and the value is a dynamic object containing a <see cref="Name"/> property.</param>
        /// <returns>
        /// The Firebase ID (key) if a match is found, or <c>null</c> if no match is found.
        /// </returns>
        /// <remarks>
        /// The search is case-insensitive. The method will return the first matching key it finds.
        /// </remarks>
        public static string? GetFirebaseIdByName(
            string name,
            Dictionary<string, dynamic> dictionary)
        {
            return dictionary
                .FirstOrDefault(x =>
                    x.Value.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                .Key;
        }
    }
}
