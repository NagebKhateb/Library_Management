using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class OwnerDto
    {
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
    }
}
