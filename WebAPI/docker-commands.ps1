# Build images
docker compose build

# Start containers
docker compose up -d
#	-v removes old volumes — avoids leftover DB files causing issues.
docker compose down -v

# Build and start
docker compose up -d --build

# Force recreate containers
docker compose up -d --force-recreate

# Build + recreate everything
docker compose up -d --build --force-recreate

# Stop containers
docker compose stop

# Stop and remove containers + network
docker compose down

# View running containers
docker ps

# View logs
docker compose logs

# View logs (live)
docker compose logs -f

#.\docker-commands.ps1   -- run this command in powershell