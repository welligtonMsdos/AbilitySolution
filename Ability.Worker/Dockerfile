# 1. Base com Playwright (Necessário para rodar o navegador no Linux)
FROM mcr.microsoft.com/playwright/dotnet:v1.49.0-noble AS base
WORKDIR /app

# 2. SDK para compilar o código
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# COPIAR OS PROJETOS (Caminhos baseados no contexto da RAÍZ da solução)
COPY ["Ability.Worker/Ability.Worker.csproj", "Ability.Worker/"]
COPY ["Ability.Domain/Ability.Domain.csproj", "Ability.Domain/"]
COPY ["Ability.Infrastructure/Ability.Infrastructure.csproj", "Ability.Infrastructure/"]

# Restaurar dependências do Worker (isso puxa as das libs automaticamente)
RUN dotnet restore "Ability.Worker/Ability.Worker.csproj"

# Copiar o código fonte de todos os projetos necessários
COPY Ability.Worker/ Ability.Worker/
COPY Ability.Domain/ Ability.Domain/
COPY Ability.Infrastructure/ Ability.Infrastructure/

WORKDIR "/src/Ability.Worker"
RUN dotnet build "Ability.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# 3. Publicação
FROM build AS publish
RUN dotnet publish "Ability.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# 4. Execução Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# INSTALAÇÃO DO NAVEGADOR (Apenas o Chromium para poupar espaço)
# O nome da DLL deve ser o nome do projeto de saída
RUN dotnet Ability.Worker.dll install chromium

ENTRYPOINT ["dotnet", "Ability.Worker.dll"]