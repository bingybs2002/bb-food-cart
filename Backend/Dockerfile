# Dev-mode Dockerfile — matches ASPNETCORE_ENVIRONMENT=Development in docker-compose.yml
FROM mcr.microsoft.com/dotnet/sdk:10.0
 
WORKDIR /app
 
# Restore first (separate layer so `npm install`-style caching works on rebuilds)
COPY *.sln ./
COPY . .
RUN dotnet restore
 
EXPOSE 8080
 
# If you have more than one .csproj in this folder, replace this with:
#   dotnet run --project YourProject.csproj --urls http://+:8080
ENTRYPOINT ["dotnet", "run", "--urls", "http://+:8080", "--no-launch-profile"]
 