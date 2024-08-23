# Dotnet Database
## Pre-requisites
```bash
dotnet tool install --global dotnet-ef
# If already installed
dotnet tool update --global dotnet-ef
```
## Migrations
### Create Migration
Default add migrations command:
```bash
dotnet ef migrations add InitialCreate
```

When the migrations is saved in another project:
```bash
dotnet ef migrations add InitialCreate --context MantaRayPlanDbContext --project ../Server.DataAccess.Database -o ../Server.DataAccess.Database/Migrations
```
### Remove Migration
```bash
dotnet ef migrations remove 
```