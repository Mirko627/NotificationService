# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Dichiara gli ARG PRIMA di usarli
ARG MIRKO_NUGET_USERNAME
ARG MIRKO_NUGET_PWD

# Esporta come variabili d'ambiente così NuGet.config le legge con %...%
ENV MIRKO_NUGET_USERNAME=$MIRKO_NUGET_USERNAME
ENV MIRKO_NUGET_PWD=$MIRKO_NUGET_PWD

WORKDIR /NotificationService

COPY . .

RUN dotnet restore /NotificationService/NotificationService.API/NotificationService.API.csproj \
    --configfile /NotificationService/NuGet.config

RUN dotnet publish /NotificationService/NotificationService.API/NotificationService.API.csproj \
    -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "NotificationService.API.dll"]