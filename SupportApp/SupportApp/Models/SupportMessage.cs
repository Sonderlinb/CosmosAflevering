using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SupportApp.Models
{
    public class SupportMessage
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        // Cosmos kræver feltet 'category'
        [JsonProperty("category")]
        [Required]
        public string Category { get; set; } = string.Empty;

        // Cosmos kræver feltet 'description'
        [JsonProperty("description")]
        [Required]
        public string Description { get; set; } = string.Empty;

        [JsonProperty("date")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonProperty("customer")]
        public CustomerInfo Customer { get; set; } = new();

        // Hjælpefelter til formular (ikke gemmes i Cosmos)
        [JsonIgnore]
        public string Name
        {
            get => Customer?.Name ?? string.Empty;
            set
            {
                if (Customer == null) Customer = new CustomerInfo();
                Customer.Name = value;
            }
        }

        [JsonIgnore]
        public string Email
        {
            get => Customer?.Email ?? string.Empty;
            set
            {
                if (Customer == null) Customer = new CustomerInfo();
                Customer.Email = value;
            }
        }

        [JsonIgnore]
        public string Phone
        {
            get => Customer?.Phone ?? string.Empty;
            set
            {
                if (Customer == null) Customer = new CustomerInfo();
                Customer.Phone = value;
            }
        }

        // Cosmos kræver, at partition key matcher containerens (/category)
        [JsonIgnore]
        public string PartitionKey => Category;
    }

    public class CustomerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("email")]
        public string Email { get; set; } = string.Empty;

        [JsonProperty("phone")]
        public string Phone { get; set; } = string.Empty;
    }
}
