set -e

echo "Waiting for SQL Server to be ready..."

# Keep trying until SQL Server responds
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1
do
  echo "SQL Server not ready, sleeping 5s..."
  sleep 5
done

echo "SQL Server is ready. Starting WebAPI..."
dotnet WebAPI.dll