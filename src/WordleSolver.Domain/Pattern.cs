using WordleSolver.Domain.Constraints;

namespace WordleSolver.Domain;

/// <summary>
/// A letter arrangement that satisfies the current ConstraintSet, independent of whether it appears
/// in the WordList. A position not pinned down by any Constraint is rendered as Placeholder rather
/// than expanded across the alphabet — Pattern exists to reveal shape, not to enumerate every word.
/// </summary>
public sealed class Pattern
{
    public const char Placeholder = '_';
    private const int WordLength = Guess.Length;

    public IReadOnlyList<char> Letters { get; }

    private Pattern(IReadOnlyList<char> letters) => Letters = letters;

    public override string ToString() => new(Letters.ToArray());

    public static IReadOnlyList<Pattern> GenerateFrom(ConstraintSet constraintSet)
    {
        ArgumentNullException.ThrowIfNull(constraintSet);

        var fixedPositions = constraintSet.Constraints
            .OfType<PositionConstraint>()
            .ToDictionary(constraint => constraint.Position, constraint => constraint.Letter);

        var excludedByPosition = new HashSet<char>[WordLength];
        for (var position = 0; position < WordLength; position++)
            excludedByPosition[position] = [];

        foreach (var exclusion in constraintSet.Constraints.OfType<PositionExclusionConstraint>())
            excludedByPosition[exclusion.Position].Add(exclusion.Letter);

        var remainingCounts = new Dictionary<char, int>();
        foreach (var minimum in constraintSet.Constraints.OfType<MinimumLetterCountConstraint>())
            remainingCounts[minimum.Letter] = Math.Max(remainingCounts.GetValueOrDefault(minimum.Letter), minimum.MinimumCount);
        foreach (var exact in constraintSet.Constraints.OfType<ExactLetterCountConstraint>())
            remainingCounts[exact.Letter] = exact.ExactCount;

        // A letter pinned to a fixed position already accounts for one of its own required occurrences.
        // If that drives a letter's remaining count negative — e.g. a fixed Correct position
        // contradicts a later exact-count-zero constraint on the same letter — the ConstraintSet
        // is unsatisfiable and no Pattern can exist.
        var results = new List<Pattern>();
        foreach (var letter in fixedPositions.Values)
        {
            if (!remainingCounts.TryGetValue(letter, out var count))
                continue;

            remainingCounts[letter] = count - 1;
            if (remainingCounts[letter] < 0)
                return results;
        }

        var placeholderBudget = WordLength - fixedPositions.Count - remainingCounts.Values.Sum();
        if (placeholderBudget < 0)
            return results;

        Backtrack(0, new char[WordLength], remainingCounts, fixedPositions, excludedByPosition, placeholderBudget, results);
        return results;
    }

    private static void Backtrack(
        int position,
        char[] pattern,
        Dictionary<char, int> remainingCounts,
        IReadOnlyDictionary<int, char> fixedPositions,
        HashSet<char>[] excludedByPosition,
        int placeholdersRemaining,
        List<Pattern> results)
    {
        if (position == WordLength)
        {
            if (remainingCounts.Values.All(count => count == 0))
                results.Add(new Pattern(pattern.ToArray()));
            return;
        }

        if (fixedPositions.TryGetValue(position, out var fixedLetter))
        {
            pattern[position] = fixedLetter;
            Backtrack(position + 1, pattern, remainingCounts, fixedPositions, excludedByPosition, placeholdersRemaining, results);
            return;
        }

        foreach (var letter in remainingCounts.Keys.ToList())
        {
            if (remainingCounts[letter] == 0 || excludedByPosition[position].Contains(letter))
                continue;

            pattern[position] = letter;
            remainingCounts[letter]--;
            Backtrack(position + 1, pattern, remainingCounts, fixedPositions, excludedByPosition, placeholdersRemaining, results);
            remainingCounts[letter]++;
        }

        if (placeholdersRemaining > 0)
        {
            pattern[position] = Placeholder;
            Backtrack(position + 1, pattern, remainingCounts, fixedPositions, excludedByPosition, placeholdersRemaining - 1, results);
        }
    }
}
