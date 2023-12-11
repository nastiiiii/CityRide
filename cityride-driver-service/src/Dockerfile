FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src/
COPY . .
RUN dotnet restore && dotnet publish -c Release -o publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS prod
WORKDIR /app/CityRide.DriverService/
COPY --from=build /src/publish .
EXPOSE 5003
EXPOSE 5004
ENTRYPOINT ["dotnet", "CityRide.DriverService.API.dll"]