using System.Text.Json.Serialization;

namespace ToDoApp.Api.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        // Foreign Key to link to a User
        [JsonIgnore]
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
