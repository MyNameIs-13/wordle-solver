namespace WordleSolver.Domain.Tests;

public class PatternTests
{
    private static ConstraintSet ConstraintsFrom(string guess, params Clue[] clues)
    {
        var history = new GuessHistory();
        history.Add(GuessRow.Create(Guess.Create(guess), clues));
        return ConstraintSet.FromHistory(history);
    }

    [Fact]
    public void SingleFixedLetter_YieldsOnePatternWithPlaceholdersElsewhere()
    {
        var constraintSet = ConstraintsFrom("STARE",
            Clue.Absent, Clue.Absent, Clue.Correct, Clue.Absent, Clue.Absent);

        var patterns = Pattern.GenerateFrom(constraintSet);

        var pattern = Assert.Single(patterns);
        Assert.Equal("__A__", pattern.ToString());
    }

    [Fact]
    public void KnownLetterExcludedFromOnePosition_EnumeratesEveryOtherPosition()
    {
        var constraintSet = ConstraintsFrom("TRAIN",
            Clue.Present, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent);

        var patterns = Pattern.GenerateFrom(constraintSet)
            .Select(p => p.ToString())
            .ToHashSet();

        Assert.Equal(["_T___", "__T__", "___T_", "____T"], patterns);
    }

    [Fact]
    public void EveryPatternSatisfiesTheConstraintSetAsARealWordWould()
    {
        var constraintSet = ConstraintsFrom("CRANE",
            Clue.Absent, Clue.Absent, Clue.Correct, Clue.Absent, Clue.Present);

        var patterns = Pattern.GenerateFrom(constraintSet);

        Assert.NotEmpty(patterns);
        foreach (var pattern in patterns)
        {
            Assert.True(constraintSet.IsSatisfiedBy(pattern.ToString()));
            Assert.Equal('A', pattern.Letters[2]);
            Assert.Contains('E', pattern.Letters);
        }
    }

    [Fact]
    public void UnsatisfiableConstraints_YieldNoPatterns()
    {
        // Position 0 is fixed to 'A' (Correct) by the first row, but the second row's exact-count
        // constraint says 'A' must not appear at all — no arrangement can satisfy both.
        var history = new GuessHistory();
        history.Add(GuessRow.Create(Guess.Create("APPLE"),
            [Clue.Correct, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent]));
        history.Add(GuessRow.Create(Guess.Create("BEACH"),
            [Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent]));

        var constraintSet = ConstraintSet.FromHistory(history);

        Assert.Empty(Pattern.GenerateFrom(constraintSet));
    }
}
