# Migrations
Execute the following command from the '**src**' folder to add migrations
```
dotnet ef migrations add <<MIGRATION_NAME>> --project .\HomeStream.Infrastructure\HomeStream.Infrastructure.csproj --startup-project .\HomeStream.Api\HomeStream.Api.csproj
```
# Redis
To start redis server from WSL
```
sudo systemctl start redis-server
```