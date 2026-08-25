namespace WordleSolver.Domain;

/// <summary>
/// The status Wordle assigned to a Tile after a Guess was submitted.
/// </summary>
public enum Clue
{
    Correct,
    Present,
    Absent
}
