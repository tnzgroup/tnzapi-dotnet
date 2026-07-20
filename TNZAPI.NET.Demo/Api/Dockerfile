FROM mcr.microsoft.com/dotnet/sdk:10.0
WORKDIR /src
COPY . .
WORKDIR /src/TNZAPI.NET.Demo/Api
EXPOSE 5080
ENV ASPNETCORE_URLS=http://+:5080
# This is a dev-only image (dotnet watch, not a Release publish) — Development is the correct
# fixed environment, so appsettings.Development.json (bind-mounted, gitignored) loads the same
# way it does under `dotnet run`/F5 locally. --no-launch-profile below skips launchSettings.json
# only (its applicationUrl/profile-specific env vars don't apply in a container), not this.
ENV ASPNETCORE_ENVIRONMENT=Development
CMD ["dotnet", "watch", "run", "--no-launch-profile"]