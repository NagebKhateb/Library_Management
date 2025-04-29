using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class StackOverflowResponseDto
    {
        [JsonPropertyName("items")]
        public List<QuestionDto> Items { get; set; } = new();
    }
}
