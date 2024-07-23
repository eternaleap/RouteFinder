FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5031

ENV ASPNETCORE_URLS=http://+:5031

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["RouteFinder.Api/RouteFinder.Api.csproj", "RouteFinder.Api/"]
RUN dotnet restore "RouteFinder.Api/RouteFinder.Api.csproj"
COPY . .
WORKDIR "/src/RouteFinder.Api"
RUN dotnet build "RouteFinder.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "RouteFinder.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RouteFinder.Api.dll"]
