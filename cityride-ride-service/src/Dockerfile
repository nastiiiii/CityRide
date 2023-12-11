FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src/
COPY . .
RUN dotnet restore && dotnet publish -c Release -o publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS prod
WORKDIR /app/CityRide.RideService/
COPY --from=build /src/publish .
EXPOSE 5007
EXPOSE 5008
ENTRYPOINT ["dotnet", "CityRide.RideService.API.dll"]