FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Ability.Api/Ability.Api.csproj", "Ability.Api/"]
COPY ["Ability.Domain/Ability.Domain.csproj", "Ability.Domain/"]
COPY ["Ability.Infrastructure/Ability.Infrastructure.csproj", "Ability.Infrastructure/"]

RUN dotnet restore "Ability.Api/Ability.Api.csproj"

COPY Ability.Api/ Ability.Api/
COPY Ability.Domain/ Ability.Domain/
COPY Ability.Infrastructure/ Ability.Infrastructure/

WORKDIR "/src/Ability.Api"
RUN dotnet build "Ability.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Ability.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Ability.Api.dll"]