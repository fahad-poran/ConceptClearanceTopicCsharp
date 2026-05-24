# Self-Hosted Runner Setup Guide

This guide explains how to register a GitHub Actions self-hosted runner on your Windows PC for this project.

## What We Are Building

We want GitHub Actions to deploy the backend automatically to a Windows machine on your local network.

In this repo, the backend is:

- `InterviewPrep.Web`
- .NET 8 ASP.NET Core app
- deployed from GitHub to your Windows PC without manual file copying

The Windows PC will act like a private test server.

## Recommended Names

When GitHub asks for the runner name and description, use these values:

- Runner name: `InterviewPrepWeb-Windows-01`
- Description: `Windows test server for InterviewPrep.Web backend`

These names are only for identification. They do not change how the runner works.

## Where You Will Need The Runner Name

You will see the runner name in:

- GitHub repository settings under `Settings` > `Actions` > `Runners`
- The list of available runners
- Logs when GitHub Actions sends a job to that machine

You will need it mainly to recognize which Windows computer is which.

## Labels To Use

When GitHub asks for labels, make sure the runner has:

- `self-hosted`
- `windows`
- `x64`

These labels match the workflow in this repository.

## Step-by-Step Setup

### 1. Open The Repository In GitHub

Go to your GitHub repository:

- `ConceptClearanceTopicCsharp`

Then open:

- `Settings`
- `Actions`
- `Runners`

### 2. Create A New Runner

Click:

- `New self-hosted runner`

Choose:

- Operating system: `Windows`
- Architecture: `x64`

### 3. Download The Runner Package

GitHub will show commands for downloading and configuring the runner.

On the Windows PC:

- create a folder such as `C:\actions-runner`
- download the runner package into that folder
- extract the files there

### 4. Run The Configuration Command

GitHub will provide a command that starts with `config.cmd`.

When the command prompts you:

- enter the runner name: `InterviewPrepWeb-Windows-01`
- enter the runner description: `Windows test server for InterviewPrep.Web backend`
- keep the labels as `self-hosted, windows, x64`

This step connects the Windows machine to your GitHub repository.

### 5. Install The Runner As A Service

GitHub will also give a command to install the runner as a service.

Use that option so the runner starts automatically when Windows starts.

That way:

- if the PC reboots, the runner comes back online
- GitHub Actions can deploy again later without manual setup

### 6. Confirm The Runner Is Online

Go back to GitHub and check the runner status.

You want it to show:

- `Idle`

That means GitHub can send it jobs.

## App Startup Task

The runner only receives deployment jobs. The backend itself also needs to start automatically on the Windows PC.

For that, we use a Windows startup task.

Run this on the Windows PC as administrator from the repository root:

```powershell
.\deploy\windows\install-service.ps1 `
  -TaskName InterviewPrepWeb `
  -DisplayName "InterviewPrep Web" `
  -AppRoot C:\apps\InterviewPrepWeb `
  -ExeName InterviewPrep.Web.exe `
  -AppUrl http://0.0.0.0:5157
```

If you are already inside `deploy\windows`, then use:

```powershell
.\install-service.ps1 `
  -TaskName InterviewPrepWeb `
  -DisplayName "InterviewPrep Web" `
  -AppRoot C:\apps\InterviewPrepWeb `
  -ExeName InterviewPrep.Web.exe `
  -AppUrl http://0.0.0.0:5157
```

## Allow LAN Access

To test from other devices on the same network, open the app port in Windows Firewall.

Run this on the Windows PC as administrator:

```powershell
deploy\windows\configure-firewall.ps1
```

## End-to-End Flow

After setup, the flow should be:

1. You push code to `main`
2. GitHub Actions builds `InterviewPrep.Web`
3. The self-hosted Windows runner receives the deployment job
4. The deploy script copies the new files into a versioned release folder under `C:\apps\InterviewPrepWeb\releases`
5. The startup task runs the backend automatically
6. You open `http://<windows-ip>:5157` from another device on your LAN

If you update `install-service.ps1` or `run-app.ps1`, run the install script again so the Windows PC gets the updated launcher copy.

## Quick Troubleshooting

- If the runner is offline, confirm the Windows PC is on and the runner service is running.
- If deployment fails, check the job logs in GitHub Actions.
- If the backend is not reachable from another device, verify the firewall rule and the port `5157`.
- If the app does not start after deployment, check that `InterviewPrep.Web.exe` exists in `C:\apps\InterviewPrepWeb`.
