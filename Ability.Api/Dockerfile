FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# VOCÊ PRECISA COPIAR OS PROJETOS DEPENDENTES AQUI TAMBÉM
COPY ["Ability.Api/Ability.Api.csproj", "Ability.Api/"]
COPY ["Ability.Domain/Ability.Domain.csproj", "Ability.Domain/"]
COPY ["Ability.Infrastructure/Ability.Infrastructure.csproj", "Ability.Infrastructure/"]

# Restaura as dependências
RUN dotnet restore "Ability.Api/Ability.Api.csproj"

# Copia o código-fonte de todos os projetos
COPY Ability.Api/ Ability.Api/
COPY Ability.Domain/ Ability.Domain/
COPY Ability.Infrastructure/ Ability.Infrastructure/

WORKDIR "/src/Ability.Api"
RUN dotnet build "Ability.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio de Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ability.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ability.Api.dll"]