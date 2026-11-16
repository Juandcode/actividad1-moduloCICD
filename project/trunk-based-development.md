# Trunk-Based Development

The project follows **Trunk-Based Development (TBD)**: all developers integrate their work into a single trunk — `main` — in small, frequent batches.

## Principles

1. **`main` is the only long-lived branch.** It is always releasable: green CI, tests passing, no half-broken work. There are no `develop`, `release/*` or long-lived environment branches.
2. **Short-lived branches only.** Any branch (`feature/`, `fix/`, `chore/`, `hotfix/`) lives **at most 1–2 days** and merges back to `main`. If a change can't be finished that fast, slice it into smaller increments.
3. **Small batches.** Integrate at least daily. Each PR is a small, releasable increment — not a feature-complete monster.
4. **Integrate before "done".** Incomplete user-visible features go to `main` **behind a feature flag**, never on a shelf branch.
5. **Fix forward.** If `main` breaks, fixing it is the highest priority (a `hotfix/` branch merged ASAP). Revert only if the fix would take too long.
6. **Releases are tags on `main`.** No release branches: cut releases by tagging `v<semver>` from trunk. The pipeline later builds from the tag.

## Branch lifetime rules

| Rule | Value |
|---|---|
| Maximum branch lifetime | 1–2 days |
| Maximum open PR size | ~400 lines changed (guideline) |
| Rebase on `origin/main` | Before opening the PR, and daily while it's open |
| Merge method | Squash merge; delete the branch immediately after |

## Incomplete work: feature flags

Anything not ready for users must be inert on `main`:

- ASP.NET Core feature flags via configuration (`FeatureManagement` or a simple `IConfiguration` flag / feature services). Disabled by default in production (`appsettings.json`), enabled locally via `appsettings.Development.json` or environment variables.
- Domain logic that must not run yet is simply not exposed: don't register the controller route or handler wiring until the feature is complete.
- Never merge code that breaks existing endpoints, migrations already applied, or the Outbox contract — TBD only works if trunk is safe.

## Direct pushes to `main`

- Default: **all changes via short-lived branch + PR** (CI + review gate).
- Exception: trivial, zero-risk changes (typos, docs, formatting) may be pushed directly to `main` by a single commit. Everything else goes through a PR.

## How TBD coexists with feature worktrees

Worktrees are a **local convenience**, not a branching model. The TBD rules apply on top:

- One worktree = one task slice = one short-lived branch.
- The worktree's branch is created from `origin/main`, merged back within days, and removed after merge (see [git-workflow.md](git-workflow.md)).
- If you are working in a worktree and the branch grows stale (> 2 days), slice the work: merge what's shippable and start a fresh worktree for the rest.

## What TBD requires from you

- `git pull --rebase origin main` before starting any work.
- Commit small and often; push the branch the same day.
- Keep PRs small enough that they can be reviewed quickly and merged the same/next day.
- If CI on `main` is red: stop feature work, fix or revert first.
