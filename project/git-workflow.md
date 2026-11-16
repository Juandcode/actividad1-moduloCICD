# Git Workflow

Operational git procedure. The branching model is **Trunk-Based Development** — read [trunk-based-development.md](trunk-based-development.md) first. Naming and message rules are in [git-conventions.md](git-conventions.md).

## Trunk rules

- `main` is the single trunk, protected by CI: every push/PR triggers build + tests + coverage (`.github/workflows/tests.yml`).
- Trunk must stay releasable at all times. If it breaks: fix forward immediately (`hotfix/<short-name>`), highest priority.
- No long-lived branches: no `develop`, no `release/*`. Releases are tags on `main`.

## Short-lived branches

- Types: `feature/`, `fix/`, `chore/`, `hotfix/` (naming rules in [git-conventions.md](git-conventions.md)).
- Created from the latest `origin/main`; lifetime **≤ 1–2 days**; removed right after merge.
- All changes go through a branch + PR, except trivial zero-risk changes (typos/docs) which may be pushed directly.

## Feature worktrees (project rule)

**Every task is developed in its own git worktree with its own short-lived branch.** One task slice = one worktree = one branch = one PR, merged back to trunk within days.

### Creating a feature worktree

Worktrees live in a sibling folder of the repo, never inside it (avoids `bin/obj`/IDE noise):

```bash
# from the main repo folder
git pull --rebase origin main
git worktree add ../actividad1-worktrees/<short-name> -b feature/<short-name> origin/main
```

Example:

```bash
git worktree add ../actividad1-worktrees/aprobar-consulta -b feature/aprobar-consulta origin/main
```

### Bootstrapping a new worktree

Worktrees don't inherit untracked files, so install the tooling once per worktree:

```bash
cd ../actividad1-worktrees/<short-name>
npm install                                        # husky pre-commit hook
dotnet restore GestionClinicaNutricionalService/GestionClinicaNutricionalService.sln
```

Verify the hook works: the `pre-commit` must run `dotnet format` on every commit in the worktree.

### Working rules inside a worktree

- Work only on that worktree's branch; one small PR per worktree (target: same/next-day merge).
- Commit small and often; push the branch the same day.
- Rebase daily on `origin/main` (`git fetch origin; git rebase origin/main`) and before opening the PR.
- Build + unit tests must pass before opening the PR.
- If the slice won't be shippable in 1–2 days, split it: merge the shippable part (behind a feature flag if needed) and open a fresh worktree for the rest.

### Closing a task

After the PR is squash-merged:

```bash
git worktree remove ../actividad1-worktrees/<short-name>
git branch -D feature/<short-name>
git worktree prune
```

Useful commands: `git worktree list`, `git worktree prune`.

## Commits

Follow the commit message convention in [git-conventions.md](git-conventions.md). In short:

- Short, imperative messages; never mix unrelated changes.
- Never commit secrets, connection strings or `.env` files.
- Do not bypass the `pre-commit` hook (`--no-verify` is forbidden).

## CI

`.github/workflows/tests.yml` runs on every push/PR to `main`: restore, tests in `Release` with coverage (`coverlet.runsettings`), ReportGenerator summary published to the run, and two artifacts (`test-results`, `coverage-report`). In TBD the trunk must stay green — if CI is red on `main`, fixing it takes priority over any new feature work.
