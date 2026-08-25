using WordleSolver.Domain.Constraints;

namespace WordleSolver.Domain;

/// <summary>
/// The aggregation of every Constraint derived from the whole GuessHistory — the accumulated
/// knowledge the solver reasons from.
/// </summary>
public sealed class ConstraintSet
{
    private readonly IReadOnlyList<Constraint> _constraints;

    private ConstraintSet(IReadOnlyList<Constraint> constraints) => _constraints = constraints;

    public IReadOnlyList<Constraint> Constraints => _constraints;

    public static ConstraintSet FromHistory(GuessHistory history)
    {
        ArgumentNullException.ThrowIfNull(history);

        var constraints = history.Rows
            .SelectMany(row => row.DeriveConstraints())
            .ToList();

        return new ConstraintSet(constraints);
    }

    public bool IsSatisfiedBy(string candidate) =>
        _constraints.All(constraint => constraint.IsSatisfiedBy(candidate));
}
