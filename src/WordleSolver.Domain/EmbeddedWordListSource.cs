using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordleSolver.Domain;

/// <summary>
/// Loads the WordList from the JSON word list embedded as a resource in this assembly
/// (the MIT-licensed darkermango/5-Letter-words list).
/// </summary>
public sealed class EmbeddedWordListSource : IWordListSource
{
    public WordList Load()
    {
        using var stream = typeof(EmbeddedWordListSource).Assembly
            .GetManifestResourceStream("WordleSolver.Domain.Resources.words.json")
            ?? throw new InvalidOperationException("Embedded word list resource 'WordleSolver.Domain.Resources.words.json' was not found.");

        var payload = JsonSerializer.Deserialize<EmbeddedWordListPayload>(stream)
            ?? throw new InvalidOperationException("Embedded word list resource was empty or malformed.");

        return new WordList(payload.Words);
    }

    private sealed class EmbeddedWordListPayload
    {
        [JsonPropertyName("words")]
        public List<string> Words { get; set; } = [];
    }
}
