FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RouteFinder.SearchServiceOne/RouteFinder.SearchServiceOne.csproj", "RouteFinder.SearchServiceOne/"]
RUN dotnet restore "RouteFinder.SearchServiceOne/RouteFinder.SearchServiceOne.csproj"
COPY . .
WORKDIR "/src/RouteFinder.SearchServiceOne"
RUN dotnet build "RouteFinder.SearchServiceOne.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RouteFinder.SearchServiceOne.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RouteFinder.SearchServiceOne.dll"]
