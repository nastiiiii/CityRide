FROM mcr.microsoft.com/dotnet/sdk AS build
WORKDIR /src/
COPY . .
RUN dotnet restore && dotnet publish -c Release -o publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS prod
WORKDIR /app/CityRide.BillingService/
COPY --from=build /src/publish .
EXPOSE 5005
EXPOSE 5006
ENTRYPOINT ["dotnet", "CityRide.BillingService.API.dll"]