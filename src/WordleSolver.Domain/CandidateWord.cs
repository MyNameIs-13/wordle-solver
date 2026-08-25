namespace WordleSolver.Domain;

/// <summary>
/// A WordList entry that satisfies the current ConstraintSet — a possible answer.
/// </summary>
public readonly record struct CandidateWord(string Word);
