# Wordle Solver

A tool that helps a player narrow down candidate Wordle answers as they play, by tracking the clues (letter colors) from each guess they've entered and filtering a dictionary against the accumulated constraints.

## Language

**Guess**:
A single 5-letter word the player has actually typed into the game, submitted as one attempt.
_Avoid_: Word, entry (too generic — use Guess when it's specifically an attempted answer)

**Tile**:
One letter-position within a Guess, carrying both the typed letter and its Clue.
_Avoid_: Cell, box

**Clue**:
The status Wordle assigned to a Tile after a Guess was submitted: Correct (right letter, right position), Present (right letter, wrong position), or Absent (letter not in the answer at that count). Corresponds to green/yellow/gray in the game's UI, but the domain names the meaning, not the color.
_Avoid_: Color, state (when talking about the domain meaning rather than UI presentation)

**GuessRow**:
One Guess plus the Clue on each of its Tiles — a single row of guess history.
_Avoid_: Attempt, line

**GuessHistory**:
The ordered sequence of GuessRows entered in the current session.
_Avoid_: Guesses, session state

**Constraint**:
A single rule a candidate answer must satisfy, derived from one or more GuessRows — e.g. "position 3 is N", "must contain A", "must not contain R", "contains exactly two N's". A repeated letter within one GuessRow that carries mixed Clues (e.g. Absent at one Tile, Correct or Present at another) yields a letter-count Constraint rather than a plain inclusion/exclusion one.
_Avoid_: Rule, filter

**ConstraintSet**:
The aggregation of every Constraint derived from the whole GuessHistory — the accumulated knowledge the solver reasons from.
_Avoid_: Known state, filters

**Dictionary**:
The fixed list of recognized 5-letter English words the solver checks candidates against.
_Avoid_: Word list (when referring to the domain concept rather than the underlying file), corpus

**CandidateWord**:
A Dictionary entry that satisfies the current ConstraintSet — a possible answer.
_Avoid_: Match, result, suggestion (this project makes no ranked suggestion — see ADR-0002)

**Pattern**:
A letter arrangement that satisfies the current ConstraintSet, independent of whether it appears in the Dictionary. Exists to reveal Constraint-satisfying possibilities the Dictionary might be missing.
_Avoid_: Possible pattern, raw word
