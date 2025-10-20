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
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string Subject { get; set; } = string.Empty;

        [Required, StringLength(2000)]
        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("partitionKey")]
        public string PartitionKey => Email ?? "unknown";
    }
}
