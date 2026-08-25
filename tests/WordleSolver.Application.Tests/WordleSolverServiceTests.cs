using WordleSolver.Domain;

namespace WordleSolver.Application.Tests;

public class WordleSolverServiceTests
{
    private static readonly WordList WordList = new EmbeddedWordListSource().Load();

    private static GuessHistory HistoryOf(params GuessRow[] rows)
    {
        var history = new GuessHistory();
        foreach (var row in rows)
            history.Add(row);
        return history;
    }

    private static GuessRow Row(string word, params Clue[] clues) =>
        GuessRow.Create(Guess.Create(word), clues);

    [Fact]
    public void FindCandidates_EmptyHistory_ReturnsWholeWordList()
    {
        var service = new WordleSolverService(WordList);

        var candidates = service.FindCandidates(HistoryOf());

        Assert.Equal(WordList.Words.Count, candidates.Count);
    }

    [Fact]
    public void FindCandidates_MatchesAgainstLowercaseWordListEntries()
    {
        // "train" is a real WordList entry (stored lowercase); pinning every letter Correct
        // must still match it despite Guess/Constraint using uppercase internally.
        var history = HistoryOf(Row("TRAIN", Clue.Correct, Clue.Correct, Clue.Correct, Clue.Correct, Clue.Correct));
        var service = new WordleSolverService(WordList);

        var candidates = service.FindCandidates(history);

        Assert.Contains(candidates, candidate => candidate.Word == "train");
    }

    [Fact]
    public void FindCandidates_ExcludesWordsViolatingConstraints()
    {
        var history = HistoryOf(Row("TRAIN", Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent));
        var service = new WordleSolverService(WordList);

        var candidates = service.FindCandidates(history);

        Assert.DoesNotContain(candidates, candidate => candidate.Word.Any("train".Contains));
    }

    [Fact]
    public void GeneratePatterns_EmptyHistory_YieldsAllPlaceholderPattern()
    {
        var service = new WordleSolverService(WordList);

        var patterns = service.GeneratePatterns(HistoryOf());

        Assert.Contains(patterns, pattern => pattern.ToString() == new string(Pattern.Placeholder, 5));
    }

    [Fact]
    public void GeneratePatterns_ReflectsFixedPositionsFromHistory()
    {
        var history = HistoryOf(Row("TRAIN", Clue.Correct, Clue.Absent, Clue.Absent, Clue.Absent, Clue.Absent));
        var service = new WordleSolverService(WordList);

        var patterns = service.GeneratePatterns(history);

        Assert.All(patterns, pattern => Assert.Equal('T', pattern.ToString()[0]));
    }
}
