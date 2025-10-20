using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SupportApp.Models
{
    public class SupportMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string Subject { get; set; }

        [Required, StringLength(2000)]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("partitionKey")]
        public string PartitionKey => Email ?? "unknown";
    }
}
