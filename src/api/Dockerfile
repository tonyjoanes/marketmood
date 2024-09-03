# Use the official .NET 8.0 SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["api/api.csproj", "."]
RUN dotnet restore "api.csproj"

# Copy everything else and build
COPY ./api .
RUN dotnet build "api.csproj" -c Release -o /app/build

# Publish the app
RUN dotnet publish "api.csproj" -c Release -o /app/publish

# Use the official .NET 8.0 runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
