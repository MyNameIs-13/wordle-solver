namespace WordleSolver.Domain.Tests;

public class GuessHistoryTests
{
    private static GuessRow Row(string word) =>
        GuessRow.Create(Guess.Create(word), [Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent]);

    [Fact]
    public void Add_AppendsToRows()
    {
        var history = new GuessHistory();
        var row = Row("CRANE");

        history.Add(row);

        Assert.Equal([row], history.Rows);
    }

    [Fact]
    public void RemoveAt_RemovesOnlyTheTargetedRow()
    {
        var history = new GuessHistory();
        var first = Row("CRANE");
        var second = Row("TOAST");
        history.Add(first);
        history.Add(second);

        history.RemoveAt(0);

        Assert.Equal([second], history.Rows);
    }
}
