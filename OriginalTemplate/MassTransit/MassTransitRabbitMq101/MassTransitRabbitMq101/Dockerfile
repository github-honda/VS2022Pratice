FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MassTransitRabbitMq101.csproj", "MassTransitRabbitMq101/"]
RUN dotnet restore "MassTransitRabbitMq101/MassTransitRabbitMq101.csproj"

COPY . MassTransitRabbitMq101/.
RUN dotnet publish -c Release -o /app MassTransitRabbitMq101/MassTransitRabbitMq101.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MassTransitRabbitMq101.dll"]