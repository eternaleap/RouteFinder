FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RouteFinder.Cache.Redis/RouteFinder.Cache.Redis.csproj", "RouteFinder.Cache.Redis/"]
RUN dotnet restore "RouteFinder.Cache.Redis/RouteFinder.Cache.Redis.csproj"
COPY . .
WORKDIR "/src/RouteFinder.Cache.Redis"
RUN dotnet build "RouteFinder.Cache.Redis.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RouteFinder.Cache.Redis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RouteFinder.Cache.Redis.dll"]
