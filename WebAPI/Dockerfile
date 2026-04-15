# -------------------------
# Base runtime image
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Make app accessible outside container
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

# Create non-root user
RUN useradd -m appuser

# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

ARG BUILD_CONFIGURATION=Release

# Copy project file first (for caching restore)
COPY WebAPI.csproj ./

# Restore dependencies
RUN dotnet restore WebAPI.csproj

# Copy all source code
COPY . .

# Build project
RUN dotnet build WebAPI.csproj -c $BUILD_CONFIGURATION -o /app/build

# -------------------------
# Publish stage
# -------------------------
FROM build AS publish

RUN dotnet publish WebAPI.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# -------------------------
# Final runtime stage
# -------------------------
FROM base AS final
WORKDIR /app

# Copy published output
COPY --from=publish /app/publish .

# Copy startup script
COPY wait-for-sqlserver.sh /wait-for-sqlserver.sh
RUN chmod +x /wait-for-sqlserver.sh

# Fix permissions for non-root user
RUN chown -R appuser /app

# Switch to non-root user
USER appuser

# Start container
ENTRYPOINT ["/wait-for-sqlserver.sh"]



































#
## This stage is used when running from VS in fast mode (Default for Debug configuration)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#
## Copy wait-for script first, as root
#COPY wait-for-sqlserver.sh /wait-for-sqlserver.sh
#RUN chmod +x /wait-for-sqlserver.sh
#
## Create a non-root user called 'appuser'
#RUN useradd -m appuser
#
## Switch to this user
#USER appuser
#
#WORKDIR /app
#EXPOSE 5000
#EXPOSE 5001
#
#
## This stage is used to build the service project
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
##COPY ["WebAPI/WebAPI.csproj", "WebAPI/"]
#COPY *.csproj ./
##RUN dotnet restore "./WebAPI/WebAPI.csproj"
#RUN dotnet restore
#COPY . .
##WORKDIR "/src/WebAPI"
#WORKDIR "/src"
#RUN dotnet build "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## This stage is used to publish the service project to be copied to the final stage
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["/wait-for-sqlserver.sh"]