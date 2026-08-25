using WordleSolver.Domain.Constraints;

namespace WordleSolver.Domain.Tests;

public class GuessRowTests
{
    private static GuessRow Row(string word, params Clue[] clues) =>
        GuessRow.Create(Guess.Create(word), clues);

    [Fact]
    public void AllCorrect_YieldsPositionAndMinimumCountConstraintsPerLetter()
    {
        var row = Row("CRANE", Clue.Correct, Clue.Correct, Clue.Correct, Clue.Correct, Clue.Correct);

        var constraints = row.DeriveConstraints();

        Assert.Contains(new PositionConstraint(0, 'C'), constraints);
        Assert.Contains(new PositionConstraint(1, 'R'), constraints);
        Assert.Contains(new PositionConstraint(2, 'A'), constraints);
        Assert.Contains(new PositionConstraint(3, 'N'), constraints);
        Assert.Contains(new PositionConstraint(4, 'E'), constraints);
        Assert.Contains(new MinimumLetterCountConstraint('C', 1), constraints);
        Assert.Contains(new MinimumLetterCountConstraint('E', 1), constraints);
    }

    [Fact]
    public void Present_YieldsPositionExclusionAndMinimumCount()
    {
        var row = Row("TRAIN", Clue.Present, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent);

        var constraints = row.DeriveConstraints();

        Assert.Contains(new PositionExclusionConstraint(0, 'T'), constraints);
        Assert.Contains(new MinimumLetterCountConstraint('T', 1), constraints);
        Assert.DoesNotContain(constraints, c => c is PositionConstraint);
    }

    [Fact]
    public void AllAbsent_YieldsExactZeroCountPerDistinctLetter()
    {
        var row = Row("STARE", Clue.Absent, Clue.Absent, Clue.Correct, Clue.Absent, Clue.Absent);

        var constraints = row.DeriveConstraints();

        Assert.Contains(new ExactLetterCountConstraint('S', 0), constraints);
        Assert.Contains(new ExactLetterCountConstraint('T', 0), constraints);
        Assert.Contains(new ExactLetterCountConstraint('R', 0), constraints);
        Assert.Contains(new ExactLetterCountConstraint('E', 0), constraints);
        Assert.Contains(new PositionConstraint(2, 'A'), constraints);
        Assert.Contains(new MinimumLetterCountConstraint('A', 1), constraints);
    }

    [Fact]
    public void RepeatedLetterWithMixedClues_YieldsExactCountNotMinimum()
    {
        // "ALLOW" against an answer with exactly one L: first L Present, second L Absent.
        var row = Row("ALLOW", Clue.Absent, Clue.Absent, Clue.Present, Clue.Absent, Clue.Absent);

        var constraints = row.DeriveConstraints();

        Assert.Contains(new ExactLetterCountConstraint('L', 1), constraints);
        Assert.DoesNotContain(constraints, c => c is MinimumLetterCountConstraint m && m.Letter == 'L');
        Assert.Contains(new PositionExclusionConstraint(2, 'L'), constraints);
    }

    [Fact]
    public void RepeatedLetterAllNonAbsent_YieldsMinimumCountNotExact()
    {
        // Both N's confirmed (one Correct, one Present) but neither carries an Absent Clue,
        // so only a lower bound is known — the answer could contain more N's than this row shows.
        var row = Row("NINJA", Clue.Correct, Clue.Absent, Clue.Present, Clue.Absent, Clue.Absent);

        var constraints = row.DeriveConstraints();

        Assert.Contains(new MinimumLetterCountConstraint('N', 2), constraints);
        Assert.DoesNotContain(constraints, c => c is ExactLetterCountConstraint e && e.Letter == 'N');
        Assert.Contains(new PositionConstraint(0, 'N'), constraints);
        Assert.Contains(new PositionExclusionConstraint(2, 'N'), constraints);
    }
}
