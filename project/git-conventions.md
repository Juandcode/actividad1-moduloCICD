# Git Conventions

Naming and message rules for branches, commits and pull requests. The operational procedure (worktrees, CI) lives in [git-workflow.md](git-workflow.md).

## Branch naming

Format: `<type>/<short-name>`

| Type | Use for | Example |
|---|---|---|
| `feature/` | New functionality | `feature/aprobar-consulta` |
| `fix/` | Bug fixes | `fix/outbox-duplicados` |
| `chore/` | Tooling, deps, docs, CI | `chore/actualizar-cpm` |
| `hotfix/` | Urgent fix of a broken trunk | `hotfix/ci-restore` |

- Lowercase, hyphen-separated, in the domain language (Spanish) or plain English.
- Created from `origin/main`, one branch per feature/worktree.
- Trunk-Based Development: branches are short-lived (**≤ 1–2 days**, merged back to `main`). There are no `develop/` or `release/*` branches.

## Commit messages

Format (Conventional Commits, adapted):

```
<type>(<scope>): <summary>

[optional body]
```

**Types**: `feat`, `fix`, `refactor`, `test`, `docs`, `chore`, `ci`.

**Scope** (optional): the feature area — `paciente`, `plan-alimenticio`, `outbox`, `tests`, `ci`, `deps`.

**Summary rules**:

- Imperative mood ("agrega", "corrige", "remove"), not "agregado/added".
- Lowercase start, no trailing period, ≤ 72 characters.
- In the domain language (Spanish) or plain English — be consistent within a PR.
- One logical change per commit; never mix unrelated changes (e.g. a refactor + a new feature).

**Body (optional)**: explain *why*, not *what*; wrap at ~100 columns.

Examples:

```
feat(paciente): agrega endpoint para aprobar consulta inicial
fix(outbox): evita duplicar mensajes al reintentar la publicacion
test(consulta): cubre caso de consulta inexistente en ApproveConsultaHandler
chore(deps): actualiza MediatR a 12.5.0 en Directory.Packages.props
ci: publica reporte de cobertura aunque fallen los tests
```

**Forbidden**:

- Generic messages with no meaning (`formatted`, `cambios`, `wip`, `asdasd`). Commits like `formatted` are not accepted going forward — describe what was actually done.
- Bypassing the husky `pre-commit` hook (`--no-verify`).
- Committing secrets, connection strings, `.env`, `bin/`, `obj/`, `node_modules/`, `TestResults/`, `coveragereport/`.

## Pull requests

- One PR = one feature worktree = one branch. Title follows the same format as a commit message.
- Keep PRs small and focused; rebase onto `origin/main` before requesting review.
- CI (`tests.yml`) must be green before merge; the PR is merged after approval.
- After merge, delete the branch and remove the worktree (see [git-workflow.md](git-workflow.md)).
