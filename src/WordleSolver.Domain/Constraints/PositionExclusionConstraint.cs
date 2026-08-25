namespace WordleSolver.Domain.Constraints;

/// <summary>
/// The candidate must not have the given letter at the given position, though the letter is known
/// to be present elsewhere — derived from a Present Clue.
/// </summary>
public sealed record PositionExclusionConstraint(int Position, char Letter) : Constraint
{
    public override bool IsSatisfiedBy(string candidate) => candidate[Position] != Letter;
}
