namespace WordleSolver.Domain;

/// <summary>
/// One letter-position within a Guess, carrying both the typed letter and its Clue.
/// </summary>
public readonly record struct Tile(char Letter, Clue Clue);
