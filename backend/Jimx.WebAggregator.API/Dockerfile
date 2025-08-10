FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./nuget.config ./
COPY ["Jimx.WebAggregator.Calculations/Jimx.WebAggregator.Calculations.csproj", "Jimx.WebAggregator.Calculations/"]
COPY ["Jimx.WebAggregator.Domain.CityCosts/Jimx.WebAggregator.Domain.CityCosts.csproj", "Jimx.WebAggregator.Domain.CityCosts/"]
COPY ["Jimx.WebAggregator.Domain.MongoDB/Jimx.WebAggregator.Domain.MongoDB.csproj", "Jimx.WebAggregator.Domain.MongoDB/"]
COPY ["Jimx.WebAggregator.Parser/Jimx.WebAggregator.Parser.csproj", "Jimx.WebAggregator.Parser/"]
COPY ["Jimx.WebAggregator.Parser.Html/Jimx.WebAggregator.Parser.Html.csproj", "Jimx.WebAggregator.Parser.Html/"]
COPY ["Jimx.WebAggregator.Persistent.MongoDB/Jimx.WebAggregator.Persistent.MongoDB.csproj", "Jimx.WebAggregator.Persistent.MongoDB/"]
COPY ["Jimx.WebAggregator.API/Jimx.WebAggregator.API.csproj", "Jimx.WebAggregator.API/"]
RUN dotnet restore "./Jimx.WebAggregator.API/Jimx.WebAggregator.API.csproj"
COPY . .
WORKDIR "/src/Jimx.WebAggregator.API"
RUN dotnet build "./Jimx.WebAggregator.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Jimx.WebAggregator.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jimx.WebAggregator.API.dll"]