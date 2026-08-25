namespace WordleSolver.Domain.Tests;

public class ConstraintSetTests
{
    [Fact]
    public void FromHistory_AggregatesConstraintsAcrossAllRows()
    {
        var history = new GuessHistory();
        history.Add(GuessRow.Create(Guess.Create("CRANE"),
            [Clue.Absent, Clue.Absent, Clue.Correct, Clue.Absent, Clue.Present]));
        history.Add(GuessRow.Create(Guess.Create("GLYPH"),
            [Clue.Correct, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent]));

        var constraintSet = ConstraintSet.FromHistory(history);

        var rowOneCount = history.Rows[0].DeriveConstraints().Count;
        var rowTwoCount = history.Rows[1].DeriveConstraints().Count;
        Assert.Equal(rowOneCount + rowTwoCount, constraintSet.Constraints.Count);
    }

    [Fact]
    public void IsSatisfiedBy_FiltersCandidatesConsistentWithEveryConstraint()
    {
        var history = new GuessHistory();
        // A at position 2 (Correct), E present but not at position 4, everything else in CRANE absent.
        history.Add(GuessRow.Create(Guess.Create("CRANE"),
            [Clue.Absent, Clue.Absent, Clue.Correct, Clue.Absent, Clue.Present]));

        var constraintSet = ConstraintSet.FromHistory(history);

        Assert.True(constraintSet.IsSatisfiedBy("LEAFY"));
        Assert.False(constraintSet.IsSatisfiedBy("CRAWL")); // contains excluded C
        Assert.False(constraintSet.IsSatisfiedBy("PLAZA")); // missing required E
        Assert.False(constraintSet.IsSatisfiedBy("LEASE")); // E back at excluded position 4
    }

    [Fact]
    public void IsSatisfiedBy_EnforcesExactCountFromMixedCluesWithinARow()
    {
        var history = new GuessHistory();
        // "ALLOW" with one L Present, one L Absent: exactly one L in the answer.
        history.Add(GuessRow.Create(Guess.Create("ALLOW"),
            [Clue.Absent, Clue.Absent, Clue.Present, Clue.Absent, Clue.Absent]));

        var constraintSet = ConstraintSet.FromHistory(history);

        Assert.False(constraintSet.IsSatisfiedBy("SKILL")); // two L's, exceeds exact count 1
        Assert.True(constraintSet.IsSatisfiedBy("GLIDE")); // one L, elsewhere than position 2
        Assert.False(constraintSet.IsSatisfiedBy("BRISK")); // zero L's, violates exact count 1
    }
}
