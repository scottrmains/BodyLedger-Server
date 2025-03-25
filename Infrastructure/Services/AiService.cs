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
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _endpoint;

        public AiService(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Gemini:ApiKey"];
            _endpoint = configuration["Gemini:BaseUrl"];
        }

        public async Task<TResult> GenerateContent<TResult>(
            string prompt,
            JsonSerializerOptions? jsonOptions = null,
            double temperature = 0.7,
            int maxOutputTokens = 8192,
            CancellationToken cancellationToken = default)
        {
            // Construct the Gemini API request endpoint
            var endpoint = _endpoint + _apiKey;

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
                    temperature,
                    topK = 64,
                    topP = 0.95,
                    maxOutputTokens,
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
                    return default;
                }

                // Clean up the raw JSON string
                textContent = textContent.Replace("\\n", "")
                                       .Replace("\\\"", "\"")
                                       .Replace("\\", "");

                // If the content has leading/trailing quotes, remove them
                if (textContent.StartsWith("\"") && textContent.EndsWith("\""))
                {
                    textContent = textContent.Substring(1, textContent.Length - 2);
                }

                // Use provided options or create default ones
                var options = jsonOptions ?? new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                return JsonSerializer.Deserialize<TResult>(textContent, options);
            }
            catch (Exception ex)
            {
                // Log the error appropriately (consider using ILogger instead of Console)
                Console.WriteLine($"Error parsing Gemini response: {ex.Message}");
                Console.WriteLine($"Response content: {responseContent}");
                return default;
            }
        }
    }
}
