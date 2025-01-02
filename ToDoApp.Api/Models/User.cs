using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ToDoApp.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }

        // Include the ICollection relationship with TodoItem
        [JsonIgnore]  // Prevents the Items property from being serialized when returning User objects in API responses.
        public ICollection<TodoItem>? TodoItems { get; set; }
    }
}
