FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
CMD ["dotnet", "Gump.WebApi.dll"]
