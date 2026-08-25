namespace WordleSolver.Domain;

/// <summary>
/// The ordered sequence of GuessRows entered in the current session.
/// </summary>
public sealed class GuessHistory
{
    private readonly List<GuessRow> _rows = [];

    public IReadOnlyList<GuessRow> Rows => _rows;

    public void Add(GuessRow row)
    {
        ArgumentNullException.ThrowIfNull(row);
        _rows.Add(row);
    }

    public void RemoveAt(int index) => _rows.RemoveAt(index);
}
