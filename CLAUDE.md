## Tech stack & architecture

C# / Blazor. Structure all code as Clean Architecture with DDD: the domain layer has no framework or infrastructure dependencies, Blazor components are thin (view only, no business logic), and an application layer orchestrates domain logic between the two.

Enforce SRP and DIP hard: give every class one reason to change, and depend on abstractions injected in — never `new` up infrastructure (data access, external services) from domain or application code.

## Agent skills

### Issue tracker

Issues live in this repo's GitHub Issues (via the `gh` CLI). See `docs/agents/issue-tracker.md`.

### Triage labels

Default canonical labels (`needs-triage`, `needs-info`, `ready-for-agent`, `ready-for-human`, `wontfix`). See `docs/agents/triage-labels.md`.

### Domain docs

Single-context layout (`CONTEXT.md` + `docs/adr/` at repo root). See `docs/agents/domain.md`.
