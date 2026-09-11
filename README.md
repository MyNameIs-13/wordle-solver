# Wordle Solver

Wordle Solver is a local web application that helps you narrow down possible
Wordle answers while you play. Enter each five-letter guess and set every
tile's result to absent (gray), present (yellow), or correct (green). The app
uses those clues to show matching words from its built-in word list. The
**Show Pattern** tab also displays letter patterns that satisfy the same clues,
including patterns not present in that list.

## Prerequisites

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download). Confirm the
installation from a terminal:

```bash
dotnet --version
```

The version reported should start with `10.`.

## Build and run

From the repository root, restore dependencies and compile the solution:

```bash
dotnet restore
dotnet build
```

Run the web application:

```bash
dotnet run --project src/WordleSolver.Web
```

The terminal prints one or more local URLs once the application is ready.
Open one of them in a browser (normally an `https://localhost:...` address).
Press `Ctrl+C` in the terminal to stop the server.

## Run the tests

```bash
dotnet test
```

## Project structure

- `src/WordleSolver.Domain` — Wordle rules, clues, constraints, and word-list types.
- `src/WordleSolver.Application` — candidate filtering and pattern generation.
- `src/WordleSolver.Web` — the Blazor Server user interface.
- `tests/` — domain and application test projects.

The `docs/` directory contains contributor and agent-facing project material,
including the domain vocabulary and issue-tracker conventions.
