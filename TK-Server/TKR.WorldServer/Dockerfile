#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TKR.WorldServer/TKR.WorldServer.csproj", "TKR.WorldServer/"]
RUN dotnet restore "TKR.WorldServer/TKR.WorldServer.csproj"
COPY . .
WORKDIR "/src/TKR.WorldServer"
RUN dotnet build "TKR.WorldServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TKR.WorldServer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TKR.WorldServer.dll"]