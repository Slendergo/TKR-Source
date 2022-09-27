FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY TKR.WorldServer/TKR.WorldServer.csproj .
COPY TKR.Shared/TKR.Shared.csproj .

RUN dotnet restore

# copy and publish app and libraries
COPY . .

RUN dotnet publish -c Release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:6.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "TKR.WorldServer.dll"]