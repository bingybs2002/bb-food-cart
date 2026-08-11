### PostgreSQL 
service postgresql start

sudo -u postgres psql

create user bing with password 'bing';

alter user "bing" superuser;

create database "bb-food" owner bing;

\q 

### Dotnet
dotnet build

dotnet-ef database update

dotnet dev-certs https --trust

echo 'export DOTNET_GENERATE_ASPNET_CERTIFICATE=false' >> ~/.bashrc

source ~/.bashrc



# Run the seeder

sudo cp /app/Backend/Testing/Miscellaneous/busy_week_transactions.sql /var/lib/postgresql

sudo -u postgres psql bb-food

\i /var/lib/postgresql/busy_week_transactions.sql

\q

### Run Dotnet backend 
dotnet run --urls http://0.0.0.0:3000 &

### Run the Admin Client
cd /app/Admin-Client/
pnpm exec vite --host 0.0.0.0 --port 3001 &

