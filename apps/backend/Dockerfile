FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
EXPOSE 80
EXPOSE 443

COPY *.sln .
COPY src/Gump.Data/*.csproj ./src/Gump.Data/
COPY src/Gump.WebApi/*.csproj ./src/Gump.WebApi/
RUN dotnet restore src/Gump.WebApi/Gump.WebApi.csproj

COPY src/. ./src/
WORKDIR /source/src/Gump.WebApi
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
CMD ["dotnet", "Gump.WebApi.dll"]
