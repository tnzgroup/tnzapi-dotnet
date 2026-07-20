# TNZAPI.NET.Demo

A working demo of `TNZAPI.NET`, the .NET SDK for the TNZ Group messaging API. It's two pieces:

- **`Api/`** — an ASP.NET Core Web API backend that uses `TNZAPI.NET` to talk to TNZ's real API. It holds your Auth Token server-side — the browser never sees it.
- **`Web/`** — a React web page (frontend) that talks only to the backend above, never to TNZ's API directly.

This guide assumes you've never used Docker before. If you have, skip to [Quick Start](#quick-start).

> ⚠️ This demo is for local development and evaluation only — see the warning banner on its own Settings page for why it should never be pointed at a production deployment. `Api/` also has no request-level authentication of its own (see `Program.cs`) and trusts whatever Auth Token it's configured with, so never run it anywhere reachable beyond your own machine.

## What you'll need

1. **Docker Desktop** — this packages up both pieces above (the .NET backend and the React frontend) so you don't need .NET or Node.js installed on your machine at all. Everything runs inside isolated containers that Docker manages for you.
2. **A TNZ Auth Token** — a JWT credential that lets the demo send real messages through your TNZ account. Find yours in the **TNZ Dashboard → Users → API**. Copy it somewhere safe; you'll paste it in during setup.

### Installing Docker Desktop

1. Go to [docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop/) and download the installer for your operating system (Windows, Mac, or Linux).
2. Run the installer and accept the defaults. On Windows, it may ask to enable WSL2 (Windows Subsystem for Linux) — accept this if prompted, it's required.
3. Once installed, **launch Docker Desktop** and wait for it to finish starting up. You'll know it's ready when the little whale icon in your system tray (Windows, bottom-right) or menu bar (Mac, top-right) stops animating and shows as steady/idle.
4. Confirm it's working by opening a terminal (PowerShell on Windows, Terminal on Mac) and running:
   ```
   docker --version
   ```
   You should see a version number printed, e.g. `Docker version 29.6.1, build ...`. If you get a "command not found" error, Docker Desktop probably isn't finished starting yet, or your terminal needs to be reopened after installing.

That's the only software you need to install. Everything else (the .NET runtime, Node.js, all the project's dependencies) is handled automatically inside the Docker containers.

## Quick Start

All commands below assume your terminal's current directory is `TNZAPI.NET.Demo/` (this folder — the one containing this README).

### 1. Add your Auth Token

Create a new file in this folder named exactly `.env` (yes, starting with a dot, no filename before it) containing:

```
TNZ_AUTH_TOKEN=<paste your JWT Auth Token here>
```

Replace `<paste your JWT Auth Token here>` with the token you copied from the TNZ Dashboard, with no quotes around it.

This file is already excluded from git (see the repo's `.gitignore`), so your token will never accidentally get committed.

> If you don't have a token yet, you can still follow the rest of this guide — the demo will run and its Settings page will let you add a token later, or you can send a test message and see it fail cleanly with an "Unauthorized" message until you do.

### 2. Start the demo

```
docker-compose up --build
```

The first time you run this, it downloads and builds everything — expect this to take a few minutes. You'll see a lot of output scroll by; that's normal. Wait until the output settles down and you see lines like:

```
api-1  | Now listening on: http://[::]:5080
web-1  |   VITE v6.4.3  ready in 1479 ms
```

This means both pieces are up and running. Leave this terminal window open — closing it stops the demo.

> **First request after starting is sometimes slow.** The backend can take 5-10 seconds after "Now listening" before it's actually ready to answer. If your first click in the browser seems to hang, just wait a moment and try again.

### 3. Open it in your browser

Go to **[http://localhost:5373](http://localhost:5373)**.

You should see the demo's home page with a sidebar listing every TNZ messaging channel and feature. Click **SMS** under Messaging to try sending a real test message (needs a valid Auth Token — see step 1), or click **Settings** under Settings to add/change your Auth Token, or explore the other options.

### 4. Stopping the demo

Go back to the terminal where `docker-compose up` is running and press `Ctrl+C`. To fully clean up the containers afterward (optional, but tidy):

```
docker-compose down
```

To start it again later, just run `docker-compose up --build` again from this folder — it'll be much faster the second time since most of the build is cached.

## Troubleshooting

**"Cannot connect to the Docker daemon" / "docker-compose: command not found"**
Docker Desktop isn't running. Launch it from your Start Menu / Applications folder and wait for the whale icon to settle before trying again.

**"port is already allocated" or the demo won't start**
Something else on your machine is already using port `5080` or `5373`. Close whatever that is, or edit the port numbers in `docker-compose.yml` (the part before the colon in each `"5080:5080"`-style line) to something free, then run `docker-compose up --build` again.

**The SMS page (or any other action) returns `"result": "Unauthorized"`**
This is expected if you haven't set a valid Auth Token yet — it means the demo correctly reached TNZ's real API and got told your credentials aren't valid, rather than something being broken. Add a real token via the `.env` file (step 1) and restart, or use the Settings page's Auth Token field to set one for your current browser session without restarting.

**I changed the `.env` file but nothing happened**
Environment variables in `.env` are only read when the containers start. Stop the demo (`Ctrl+C`, or `docker-compose down`) and run `docker-compose up --build` again to pick up the change.

**I want to point the demo at something running on my own computer (not TNZ's real API)**
From inside a Docker container, `localhost` refers to the container itself, not your computer. Docker Desktop provides a special address for this: use `http://host.docker.internal:<port>` instead of `http://localhost:<port>` in the Settings page's API URL field.

## Alternative: running without Docker

If you already have the [.NET SDK](https://dotnet.microsoft.com/download) and [Node.js](https://nodejs.org/) installed and would rather not use Docker, you can run the two pieces directly instead:

1. Set your Auth Token as a [user secret](https://learn.microsoft.com/aspnet/core/security/app-secrets) (never committed):
   ```
   dotnet user-secrets init --project Api/TNZAPI.NET.Demo.Api.csproj
   dotnet user-secrets set TNZ_AUTH_TOKEN "<your JWT AuthToken>" --project Api/TNZAPI.NET.Demo.Api.csproj
   ```
2. In one terminal, run the backend:
   ```
   dotnet run --project Api/TNZAPI.NET.Demo.Api.csproj
   ```
   It listens on `http://localhost:5080`.
3. In a second terminal, run the frontend:
   ```
   cd Web
   npm install
   npm run dev
   ```
   It listens on `http://localhost:5373` and proxies API calls through to the backend above.
4. Open `http://localhost:5373`.

You can skip step 1 entirely and use the in-app Settings page to supply a token for your session instead.

## The Settings page

The sidebar's **Settings → General** page lets you change how the backend connects to TNZ without editing any files or restarting anything:

- **Auth Token** — overrides the token for your current session only. Lost when the backend restarts (see the info note under its field) — it's deliberately never saved to your browser, even with Remember turned on, since it's a real credential.
- **API URL** — overrides which server the SDK talks to (defaults to TNZ's real API). Applies to every session on this backend instance, not just yours.
- **SSL Verification** / **Allow Insecure HTTP** — two separate safety checks you can turn off for testing against a non-production server (e.g. one with a self-signed certificate, or no TLS at all). Both are on/enforced by default; see the warning banner on that page for why they matter.
- **Remember These Settings** — saves API URL / SSL Verification / Allow Insecure HTTP to your browser (not the Auth Token) so they survive a backend restart without you having to re-enter them.

## Project layout

```
TNZAPI.NET.Demo/
  Api/               ASP.NET Core Web API backend (TNZAPI.NET.Demo.Api.csproj)
  Web/               React + TypeScript + Vite frontend
  docker-compose.yml Orchestrates both as separate containers
  .env               Your Auth Token (you create this — not committed)
```