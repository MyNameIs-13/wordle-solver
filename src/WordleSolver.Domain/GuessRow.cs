namespace WordleSolver.Domain;

/// <summary>
/// One Guess plus the Clue on each of its Tiles — a single row of guess history.
/// </summary>
public sealed class GuessRow
{
    public Guess Guess { get; }
    public IReadOnlyList<Tile> Tiles { get; }

    private GuessRow(Guess guess, IReadOnlyList<Tile> tiles)
    {
        Guess = guess;
        Tiles = tiles;
    }

    public static GuessRow Create(Guess guess, IReadOnlyList<Clue> clues)
    {
        ArgumentNullException.ThrowIfNull(clues);

        if (clues.Count != guess.Word.Length)
            throw new ArgumentException($"A guess row needs exactly {guess.Word.Length} clues, one per tile.", nameof(clues));

        var tiles = new Tile[guess.Word.Length];
        for (var position = 0; position < tiles.Length; position++)
            tiles[position] = new Tile(guess[position], clues[position]);

        return new GuessRow(guess, tiles);
    }

    /// <summary>
    /// Turns this row's Tiles into the Constraints they imply. A letter that appears more than once
    /// in the row with mixed Clues (some Correct/Present, at least one Absent) yields an exact-count
    /// Constraint rather than a plain minimum, since the Absent Clue(s) pin down its true count.
    /// </summary>
    public IReadOnlyList<Constraint> DeriveConstraints()
    {
        var constraints = new List<Constraint>();

        for (var position = 0; position < Tiles.Count; position++)
        {
            var tile = Tiles[position];
            switch (tile.Clue)
            {
                case Clue.Correct:
                    constraints.Add(new PositionConstraint(position, tile.Letter));
                    break;
                case Clue.Present:
                    constraints.Add(new PositionExclusionConstraint(position, tile.Letter));
                    break;
                case Clue.Absent:
                    break;
            }
        }

        foreach (var group in Tiles.GroupBy(tile => tile.Letter))
        {
            var nonAbsentCount = group.Count(tile => tile.Clue != Clue.Absent);
            var hasAbsentTile = group.Any(tile => tile.Clue == Clue.Absent);

            constraints.Add(hasAbsentTile
                ? new ExactLetterCountConstraint(group.Key, nonAbsentCount)
                : new MinimumLetterCountConstraint(group.Key, nonAbsentCount));
        }

        return constraints;
    }
}
