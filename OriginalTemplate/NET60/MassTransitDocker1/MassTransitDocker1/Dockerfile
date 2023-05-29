FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MassTransitDocker1.csproj", "MassTransitDocker1/"]
RUN dotnet restore "MassTransitDocker1/MassTransitDocker1.csproj"

COPY . MassTransitDocker1/.
RUN dotnet publish -c Release -o /app MassTransitDocker1/MassTransitDocker1.csproj 

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS publish
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "MassTransitDocker1.dll"]