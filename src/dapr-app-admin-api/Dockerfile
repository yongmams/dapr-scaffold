#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["./dapr-app-common/DaprApp.Common.csproj", "./dapr-app-common/DaprApp.Common.csproj"]
RUN dotnet restore "./dapr-app-common/DaprApp.Common.csproj"

COPY ["./dapr-app-admin-api/DapApp.Admin.API.csproj", "./dapr-app-admin-api/DapApp.Admin.API.csproj"]
RUN dotnet restore "./dapr-app-admin-api/DapApp.Admin.API.csproj"

COPY ["./dapr-app-common", "./dapr-app-common"]
COPY ["./dapr-app-admin-api", "./dapr-app-admin-api"]
WORKDIR "/src/."
RUN dotnet build "./dapr-app-admin-api/DapApp.Admin.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./dapr-app-admin-api/DapApp.Admin.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DapApp.Admin.API.dll"]