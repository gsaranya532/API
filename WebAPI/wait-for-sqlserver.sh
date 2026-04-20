#!/bin/bash
set -e

echo "Waiting for SQL Server..."

# retry until SQL is ready
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD_QA" -Q "SELECT 1"; do
  echo "SQL Server is unavailable - retrying..."
  sleep 5
done

echo "SQL Server is up - starting API"

exec dotnet WebAPI.dll