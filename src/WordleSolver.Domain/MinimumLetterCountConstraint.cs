namespace WordleSolver.Domain;

/// <summary>
/// The candidate must contain the given letter at least this many times — derived from a GuessRow
/// where every occurrence of the letter carried a Correct or Present Clue, leaving its true count unknown.
/// </summary>
public sealed record MinimumLetterCountConstraint(char Letter, int MinimumCount) : Constraint
{
    public override bool IsSatisfiedBy(string candidate) =>
        candidate.Count(letter => letter == Letter) >= MinimumCount;
}
