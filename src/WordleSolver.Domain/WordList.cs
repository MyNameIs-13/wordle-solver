namespace WordleSolver.Domain;

/// <summary>
/// The fixed list of recognized 5-letter English words the solver checks candidates against.
/// </summary>
public sealed class WordList
{
    private readonly IReadOnlyList<string> _words;

    internal WordList(IReadOnlyList<string> words)
    {
        _words = words;
    }

    public IReadOnlyList<string> Words => _words;
}
