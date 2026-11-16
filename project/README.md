# Project Rules & Conventions

Canonical rules for `actividad1-modulocicd`. Any human or AI assistant working in this repo must follow them.

## Documents

| Document | Content |
|---|---|
| [architecture.md](architecture.md) | Layers, dependency direction, CQRS, Outbox, package management |
| [conventions.md](conventions.md) | Code style, domain language, testing rules |
| [git-conventions.md](git-conventions.md) | Branch naming, commit messages, pull requests |
| [git-workflow.md](git-workflow.md) | Trunk workflow, **feature worktrees**, CI |
| [trunk-based-development.md](trunk-based-development.md) | Trunk-Based Development model, branch lifetime, feature flags |

## Core principles

1. **Trunk-Based Development.** `main` is the single, always-releasable trunk. Work is integrated in small batches through short-lived branches (≤ 1–2 days); incomplete features go behind feature flags. Never work directly on `main`.
2. **One task = one worktree.** Every new task/feature slice is developed in its own git worktree with its own short-lived branch.
3. **Dependencies point inward** (Clean Architecture / DDD). No reverse references.
4. **Tests must stay green.** Build + unit tests after every C# change; CI (`tests.yml`) must pass on every push/PR to `main`. A broken trunk is fixed before any new work.
5. **No secrets in git.** Connection strings and credentials go in `appsettings.Development.json` or environment variables, never in code.
6. **Prefer editing existing code** over creating new files or projects.

The repo-root [`AGENTS.md`](../AGENTS.md) is the machine-readable summary of these rules; if there is ever a conflict, these documents win.
