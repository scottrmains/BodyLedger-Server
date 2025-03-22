using Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DeepseekService : IDeepseekService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public DeepseekService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Deepseek:ApiKey"];
            _endpoint = configuration["Deepseek:BaseUrl"];
        }

        public async Task<List<GeneratedExercise>> GenerateWorkoutExercises(
                string workoutName,
                string description,
                CancellationToken cancellationToken)
        {
            // Create a prompt for the AI
            var prompt = $"Based on this workout named '{workoutName}' with description '{description}', " +
                         "generate a list of appropriate exercises with recommended sets and rep ranges. " +
                         "Format the response as JSON with fields: name, sets, repRange. " +
                         "Include 4-6 exercises that would make sense for this workout.";

            // Construct the Gemini API request
            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-pro-exp-02-05:generateContent?key={_apiKey}";

            var content = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    topK = 64,
                    topP = 0.95,
                    maxOutputTokens = 8192,
                    responseMimeType = "application/json"
                }
            };

            var requestContent = new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync(endpoint, requestContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            try
            {
                // Parse the JSON response
                using JsonDocument doc = JsonDocument.Parse(responseContent);

                // Navigate through the nested structure to get the text content
                var candidatesArray = doc.RootElement.GetProperty("candidates");
                var firstCandidate = candidatesArray[0];
                var contentObj = firstCandidate.GetProperty("content");
                var partsArray = contentObj.GetProperty("parts");
                var textContent = partsArray[0].GetProperty("text").GetString();

                if (string.IsNullOrEmpty(textContent))
                {
                    return new List<GeneratedExercise>();
                }

                // Clean up the raw JSON string - the issue is with escaped characters
                textContent = textContent.Replace("\\n", "")
                                        .Replace("\\\"", "\"")
                                        .Replace("\\", "");

                // If the content now has leading/trailing quotes, remove them
                if (textContent.StartsWith("\"") && textContent.EndsWith("\""))
                {
                    textContent = textContent.Substring(1, textContent.Length - 2);
                }

                // Special handling for the JSON format that Gemini returns
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                var exercises = JsonSerializer.Deserialize<List<GeneratedExercise>>(textContent, options);
                return exercises ?? new List<GeneratedExercise>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing Gemini response: {ex.Message}");
                Console.WriteLine($"Response content: {responseContent}");
                return new List<GeneratedExercise>();

            }
        }
    }
}
