#!/bin/bash

set +e

echo "Executing start.sh script..."
sudo service postgresql start

echo "Modifying PostgreSQL..."
sudo -u postgres psql -v ON_ERROR_STOP=0 <<-EOSQL
    CREATE USER bingbing WITH PASSWORD 'bing';
    ALTER USER bingbing SUPERUSER;
    CREATE DATABASE "bb-food" OWNER bingbing;
EOSQL
echo "Users and database ready."

echo "Starting .NET backend..."
cd /app/Backend

dotnet build
dotnet-ef database update

echo "seeding the database..."
dotnet run
sudo -u postgres psql -d "bb-food" -v ON_ERROR_STOP=0 -f /app/Backend/Testing/Miscellaneous/busy_week_transactions.sql
echo "Database seeded."

dotnet dev-certs https --trust
echo 'export DOTNET_GENERATE_ASPNET_CERTIFICATE=false' >> ~/.bashrc
source ~/.bashrc

wait