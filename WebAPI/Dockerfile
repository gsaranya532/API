# -------------------------
# Base runtime image
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000

RUN useradd -m appuser

# -------------------------
# Build stage
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebAPI.csproj ./
RUN dotnet restore WebAPI.csproj

COPY . .
RUN dotnet build WebAPI.csproj -c Release -o /app/build

# -------------------------
# Publish stage
# -------------------------
FROM build AS publish
RUN dotnet publish WebAPI.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# -------------------------
# Final stage
# -------------------------
#FROM base AS final
#WORKDIR /app
#
#COPY --from=publish /app/publish .
#
## Copy startup script
#COPY wait-for-sqlserver.sh /app/wait-for-sqlserver.sh
#
## FIX: ensure Linux compatibility (NO dos2unix needed)
#RUN sed -i 's/\r$//' /app/wait-for-sqlserver.sh \
    #&& chmod +x /app/wait-for-sqlserver.sh
#
#RUN chown -R appuser /app
#
#USER appuser
#
#ENTRYPOINT ["/bin/bash", "/app/wait-for-sqlserver.sh"]
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

RUN chown -R appuser /app

USER appuser

ENTRYPOINT ["dotnet", "WebAPI.dll"]

## -------------------------
## Base runtime image
## -------------------------
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#WORKDIR /app
#
## Make app accessible outside container
#ENV ASPNETCORE_URLS=http://+:5000
#EXPOSE 5000
#
## Create non-root user
#RUN useradd -m appuser
#
## -------------------------
## Build stage
## -------------------------
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
#ARG BUILD_CONFIGURATION=Release
#
## Copy project file first (for caching restore)
#COPY WebAPI.csproj ./
#
## Restore dependencies
#RUN dotnet restore WebAPI.csproj
#
## Copy all source code
#COPY . .
#
## Build project
#RUN dotnet build WebAPI.csproj -c $BUILD_CONFIGURATION -o /app/build
#
## -------------------------
## Publish stage
## -------------------------
#FROM build AS publish
#
#RUN dotnet publish WebAPI.csproj \
    #-c Release \
    #-o /app/publish \
    #/p:UseAppHost=false
#
## -------------------------
## Final runtime stage
## -------------------------
#FROM base AS final
#WORKDIR /app
#
## Copy published output
#COPY --from=publish /app/publish .
#
#
## Copy startup script
##convert Windows → Linux format (CRLF → LF)
#RUN apt-get update && apt-get install -y dos2unix \
    #&& dos2unix /app/wait-for-sqlserver.sh
#
##DEBUG LINE confirm file exist during build
#RUN ls -l /app
##&& cat /app/wait-for-sqlserver.sh
##chmod - permision
#RUN chmod +x /app/wait-for-sqlserver.sh
#
## Fix permissions for non-root user
#RUN chown -R appuser /app
#
## Switch to non-root user
#USER appuser
#
## Start container
#ENTRYPOINT ["/app/wait-for-sqlserver.sh"]