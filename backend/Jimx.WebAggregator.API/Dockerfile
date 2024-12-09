FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 55574
EXPOSE 55575

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Jimx.WebAggregator.API/Jimx.WebAggregator.API.csproj", "Jimx.WebAggregator.API/"]
RUN dotnet restore "./Jimx.WebAggregator.API/Jimx.WebAggregator.API.csproj"
COPY . .
WORKDIR "/src/Jimx.WebAggregator.API"
RUN dotnet build "./Jimx.WebAggregator.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Jimx.WebAggregator.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jimx.WebAggregator.API.dll"]