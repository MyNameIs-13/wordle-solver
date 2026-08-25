namespace WordleSolver.Domain;

/// <summary>
/// A single 5-letter word the player has actually typed into the game, submitted as one attempt.
/// </summary>
public readonly record struct Guess
{
    public const int Length = 5;

    public string Word { get; }

    private Guess(string word) => Word = word;

    public static Guess Create(string word)
    {
        ArgumentNullException.ThrowIfNull(word);

        if (word.Length != Length || !word.All(char.IsAsciiLetter))
            throw new ArgumentException($"A guess must be exactly {Length} alphabetic letters.", nameof(word));

        return new Guess(word.ToUpperInvariant());
    }

    public char this[int position] => Word[position];
}
