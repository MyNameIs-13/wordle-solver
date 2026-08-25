using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordleSolver.Domain;

/// <summary>
/// The fixed list of recognized 5-letter English words the solver checks candidates against.
/// </summary>
public sealed class WordList
{
    private readonly IReadOnlyList<string> _words;

    private WordList(IReadOnlyList<string> words)
    {
        _words = words;
    }

    public IReadOnlyList<string> Words => _words;

    public static WordList LoadEmbedded()
    {
        using var stream = typeof(WordList).Assembly
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
