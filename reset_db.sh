#!/bin/sh
rm -rf src/Backend/Pliromi.API/Migrations
dotnet ef database drop -f -v --project src/Backend/Pliromi.API

dotnet ef migrations add InitialCreate --project src/Backend/Pliromi.API

dotnet ef database update --project src/Backend/Pliromi.API
