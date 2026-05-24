# DotNet8InterviewPrep

Interview preparation solution for a **.NET developer with 3+ years experience**.
Now it includes both:
- Console learning app
- Browser-based web UI (HTML/CSS/JS)

## Projects
- `InterviewPrep.Console`: menu-driven console learning flow
- `InterviewPrep.Web`: interactive browser app with order calculator + topic cards
- `InterviewPrep.Shared`: shared domain, services, and interview catalog used by both hosts

## Browser app (new)
The web project provides a clean client view built with:
- HTML for structure
- CSS for polished responsive layout
- Vanilla JavaScript for interactivity
- ASP.NET Core Minimal API for real-time order calculation and interview content endpoints

### Features in browser
- Hero landing with guided actions
- Real-world order calculator
- Live table of cart items
- Discount rules demonstration via API
- Interview-topic navigator cards

## Run in browser
From solution root:

```bash
dotnet run --project InterviewPrep.Web --urls http://localhost:5157
```

Then open:
- `http://localhost:5157`

## Deploy to a Windows test machine

This repo includes a starter GitHub Actions deployment for a private Windows server on your local network.

### How it works

1. Push to `main`.
2. GitHub Actions publishes `InterviewPrep.Web` as a Windows self-contained app.
3. A self-hosted Windows runner on your PC downloads the build artifact.
4. A PowerShell deploy script writes the build into a versioned release folder and restarts a Windows startup task.

### Files added for deployment

- [`.github/workflows/deploy-windows.yml`](.github/workflows/deploy-windows.yml)
- [`deploy/windows/deploy.ps1`](deploy/windows/deploy.ps1)
- [`deploy/windows/install-service.ps1`](deploy/windows/install-service.ps1)
- [`deploy/windows/configure-firewall.ps1`](deploy/windows/configure-firewall.ps1)
- [`deploy/windows/runner-setup.md`](deploy/windows/runner-setup.md)

### Windows startup task details

- Task name: `InterviewPrepWeb`
- Deploy directory: `C:\apps\InterviewPrepWeb`
- Release folders: `C:\apps\InterviewPrepWeb\releases\<run-id>-<attempt>`
- Active release file: `C:\apps\InterviewPrepWeb\active-release.txt`
- LAN URL: `http://<windows-ip>:5157`
- Local URL on the server: `http://localhost:5157`

### One-time setup on the Windows machine

1. Install the self-hosted GitHub runner and give it the labels `self-hosted`, `windows`, and `x64`.
2. Run `deploy/windows/install-service.ps1` once as administrator to register the startup task.
3. Run `deploy/windows/configure-firewall.ps1` once as administrator to allow your LAN to reach the app port.
4. Keep the runner running so deployments can happen automatically on each push.

## Interview topic coverage map
- OOP: Payment model + processing flow
- CTS: value/reference + boxing/unboxing (console module)
- Tuples: discount return values
- `readonly`: `OrderItem` as readonly record struct
- Generic method/repository: console module
- Pattern matching: pricing/payment rules
- Delegates/events: console module
- .NET 8: `FrozenDictionary`, primary constructors, `TimeProvider`, `required`

## Quick Git sync for junior developers

Yes, you can use the included `sync.sh` script to push changes without typing these commands every time:

```bash
git add .
git commit -m "your message"
git push origin main
```

The script already runs those steps for you.

### One-time setup

Run this once from the project root:

```bash
chmod +x sync.sh
```

Make sure GitHub authentication is already configured on your machine. For example, you can use GitHub CLI:

```bash
gh auth login
```

### Push changes with one command

From the project root, run:

```bash
./sync.sh "your commit message"
```

Example:

```bash
./sync.sh "Add lesson pagination"
```

This will:

1. Stage all changed and new files with `git add .`.
2. Create a commit using your message.
3. Push the commit to the `main` branch on GitHub.

After the push reaches `main`, the publish deployment can run from GitHub Actions.

### Important notes

- Always check your changed files before syncing:

```bash
git status
```

- Do not use `./sync.sh` if you have private files, test files, or unfinished changes that should not be pushed.
- Write a clear commit message so other developers understand the change.
- The script pushes to `origin main`, so use it only when your change is ready for the main branch.
