using System.Text.Json;
using System.Text.Json.Serialization;

namespace WordleSolver.Domain;

/// <summary>
/// The fixed list of recognized 5-letter English words the solver checks candidates against.
/// </summary>
public sealed class Dictionary
{
    private readonly IReadOnlyList<string> _words;

    private Dictionary(IReadOnlyList<string> words)
    {
        _words = words;
    }

    public IReadOnlyList<string> Words => _words;

    public static Dictionary LoadEmbedded()
    {
        using var stream = typeof(Dictionary).Assembly
            .GetManifestResourceStream("WordleSolver.Domain.Resources.words.json")
            ?? throw new InvalidOperationException("Embedded word list resource 'WordleSolver.Domain.Resources.words.json' was not found.");

        var payload = JsonSerializer.Deserialize<WordListPayload>(stream)
            ?? throw new InvalidOperationException("Embedded word list resource was empty or malformed.");

        return new Dictionary(payload.Words);
    }

    private sealed class WordListPayload
    {
        [JsonPropertyName("words")]
        public List<string> Words { get; set; } = [];
    }
}
