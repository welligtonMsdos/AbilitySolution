FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

USER root

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgbm1 \
    libnss3 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libasound2t64 \
    libpangocairo-1.0-0 \
    libgtk-3-0 \
    libxshmfence1 \
    fonts-liberation \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Ability.Worker/Ability.Worker.csproj", "Ability.Worker/"]
COPY ["Ability.Domain/Ability.Domain.csproj", "Ability.Domain/"]
COPY ["Ability.Infrastructure/Ability.Infrastructure.csproj", "Ability.Infrastructure/"]

RUN dotnet restore "Ability.Worker/Ability.Worker.csproj"

COPY Ability.Worker/ Ability.Worker/
COPY Ability.Domain/ Ability.Domain/
COPY Ability.Infrastructure/ Ability.Infrastructure/

WORKDIR "/src/Ability.Worker"
RUN dotnet build "Ability.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ability.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools"
RUN playwright install chromium

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
COPY --from=publish /root/.cache/ms-playwright /root/.cache/ms-playwright

ENV PLAYWRIGHT_BROWSERS_PATH=/root/.cache/ms-playwright

ENTRYPOINT ["dotnet", "Ability.Worker.dll"]