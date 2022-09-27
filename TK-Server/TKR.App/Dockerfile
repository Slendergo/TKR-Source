#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TKR.App/TKR.App.csproj", "TKR.App/"]
RUN dotnet restore "TKR.App/TKR.App.csproj"
COPY . .
WORKDIR "/src/TKR.App"
RUN dotnet build "TKR.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TKR.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TKR.App.dll"]