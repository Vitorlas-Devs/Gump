#!/bin/bash
wget https://raw.githubusercontent.com/14A-A-Lyedlik-Devs/Gump/main/apps/backend/docker-compose.yml
wget https://raw.githubusercontent.com/14A-A-Lyedlik-Devs/Gump/main/apps/backend/start.sh
chmod +x ./start.sh
sudo docker compose down --volumes --remove-orphans
sudo docker compose up -d --build --force-recreate
