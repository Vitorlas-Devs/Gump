# Gump Backend

Ebben a mappában található a Gump backendje.

Asp.Net, C#, MongoDB, DigitalOcean

## Futtatás Dockerrel

Készíts egy `docker-compose.yml` fájlt.

Példa:

```yml
version: "3"

services:
  api:
    image: exatom42/gump:latest
    restart: always
    ports:
      - "3000:80"
    environment:
      - Logging__LogLevel__Default=Information
      - Logging__LogLevel__Microsoft.AspNetCore=Warning
      - AllowedHosts=*
      - MongoDbConfig__Name=gump
      - MongoDbConfig__Host=db
      - MongoDbConfig__Port=27017
      - JwtConfig__Secret=---JWT_SECRET---
      - JwtConfig__Expiration=60
      - Pepper=---TOKEN---
      - GitHubConfig__ClientId=---GITHUB_CLIENT_ID---
      - GitHubConfig__ClientSecret=---GITHUB_CLIENT_SECRET---

  db:
    image: mongo:6
    restart: always
    volumes:
      - ./data:/data/db
```

A tripla kötőjellel (`---`) jelölt részeket cseréld ki saját értékekre. Ezek titkos kulcsok, amiket nem szabad megosztani.

Az API elindításához írd be a következő parancsot:

```bash
docker compose up -d --build --force-recreate
```

Leállítani pedig ezzel a paranccsal lehet:

```bash
docker compose down --volumes --remove-orphans
```
