# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY server/HotelHaloHub.API/*.csproj ./server/HotelHaloHub.API/
RUN dotnet restore ./server/HotelHaloHub.API/HotelHaloHub.API.csproj

# Copy everything else and build
COPY server/HotelHaloHub.API/. ./server/HotelHaloHub.API/
WORKDIR /app/server/HotelHaloHub.API
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/server/HotelHaloHub.API/out .

# Render provides the PORT environment variable
ENV ASPNETCORE_URLS=http://+:$PORT

EXPOSE $PORT
ENTRYPOINT ["dotnet", "HotelHaloHub.API.dll"]
