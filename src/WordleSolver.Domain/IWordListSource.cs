namespace WordleSolver.Domain;

/// <summary>
/// Produces the WordList the solver checks candidates against, abstracting where the underlying
/// word data actually comes from (embedded resource today, potentially a remote or user-supplied
/// source later) so that mechanism can change without touching anything that consumes a WordList.
/// </summary>
public interface IWordListSource
{
    WordList Load();
}
