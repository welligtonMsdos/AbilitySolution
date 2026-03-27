FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

USER root

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
    libgbm1 \
    libnss3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxrandr2 \
    libasound2t64 \
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

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Precisamos do SDK temporariamente para instalar a ferramenta ou usar o npx
# Mas como você já tem as libs no 'base', vamos apenas garantir que o Playwright funcione
USER root

# Instala o Node.js rapidamente apenas para rodar o instalador do Playwright (mais confiável em ambientes Docker)
RUN apt-get update && apt-get install -y nodejs npm

# Instala o Playwright e os navegadores
RUN npx playwright install --with-deps chromium

ENTRYPOINT ["dotnet", "Ability.Worker.dll"]