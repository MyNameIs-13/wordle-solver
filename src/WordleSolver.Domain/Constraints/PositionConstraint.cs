namespace WordleSolver.Domain.Constraints;

/// <summary>
/// The candidate must have the given letter at the given position — derived from a Correct Clue.
/// </summary>
public sealed record PositionConstraint(int Position, char Letter) : Constraint
{
    public override bool IsSatisfiedBy(string candidate) => candidate[Position] == Letter;
}
