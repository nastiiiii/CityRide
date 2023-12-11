FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src/
COPY . .
RUN dotnet restore && dotnet publish -c Release -o publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS prod
WORKDIR /app/CityRide.ClientService/
COPY --from=build /src/publish .
EXPOSE 5001
EXPOSE 5002
ENTRYPOINT ["dotnet", "CityRide.ClientService.API.dll"]