namespace WordleSolver.Domain.Constraints;

/// <summary>
/// The candidate must contain the given letter exactly this many times — derived from a GuessRow
/// where a repeated letter carried mixed Clues (some Correct/Present, at least one Absent), which
/// pins down the letter's true count in the answer.
/// </summary>
public sealed record ExactLetterCountConstraint(char Letter, int ExactCount) : Constraint
{
    public override bool IsSatisfiedBy(string candidate) =>
        candidate.Count(letter => letter == Letter) == ExactCount;
}
