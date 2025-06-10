# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["StudentTermTrackerAPI.csproj", "./"]
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Expose port 80 for HTTP traffic for Azure App Service
EXPOSE 80
# EXPOSE 443 TOOD: Verify this isn't needed 

# Copy published output from build stage
COPY --from=build /app/publish .

# Set environment variable for Azure App Service
ENV ASPNETCORE_URLS=http://+:80

# Start App
ENTRYPOINT ["dotnet", "StudentTermTrackerAPI.dll"]