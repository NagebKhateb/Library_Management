using System.Text.Json.Serialization;

namespace Domain.DTOs
{
    public class QuestionDto
    {
        [JsonPropertyName("question_id")]
        public int QuestionId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("owner")]
        public OwnerDto Owner { get; set; } = new();

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("answer_count")]
        public int AnswerCount { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
