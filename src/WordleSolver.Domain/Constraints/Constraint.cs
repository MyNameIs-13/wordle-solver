namespace WordleSolver.Domain.Constraints;

/// <summary>
/// A single rule a candidate answer must satisfy, derived from one or more GuessRows.
/// </summary>
public abstract record Constraint
{
    public abstract bool IsSatisfiedBy(string candidate);
}
