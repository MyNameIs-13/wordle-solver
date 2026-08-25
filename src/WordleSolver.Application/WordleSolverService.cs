using WordleSolver.Domain;

namespace WordleSolver.Application;

/// <summary>
/// Orchestrates GuessHistory into ADR-0002's two-tier output: the live CandidateWord list
/// Blazor components bind to, and Patterns computed only on demand.
/// </summary>
public sealed class WordleSolverService
{
    private readonly WordList _wordList;

    public WordleSolverService(WordList wordList)
    {
        ArgumentNullException.ThrowIfNull(wordList);
        _wordList = wordList;
    }

    public IReadOnlyList<CandidateWord> FindCandidates(GuessHistory history)
    {
        var constraintSet = ConstraintSet.FromHistory(history);

        // Constraints compare against Guess's uppercase convention (see Guess.Create), while
        // WordList entries are stored lowercase, so satisfaction is checked on an uppercased
        // copy while the original casing is preserved for display.
        return _wordList.Words
            .Where(word => constraintSet.IsSatisfiedBy(word.ToUpperInvariant()))
            .Select(word => new CandidateWord(word))
            .ToList();
    }

    public IReadOnlyList<Pattern> GeneratePatterns(GuessHistory history)
    {
        var constraintSet = ConstraintSet.FromHistory(history);
        return Pattern.GenerateFrom(constraintSet);
    }
}
