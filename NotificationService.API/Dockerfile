FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG MIRKO_NUGET_USERNAME
ARG MIRKO_NUGET_PWD
ENV MIRKO_NUGET_USERNAME=$MIRKO_NUGET_USERNAME
ENV MIRKO_NUGET_PWD=$MIRKO_NUGET_PWD

WORKDIR /src

# NuGet
COPY NuGet.config .

# Copio i csproj
COPY NotificationService.API/NotificationService.API.csproj NotificationService.API/
COPY NotificationService.Business/NotificationService.Business.csproj NotificationService.Business/
COPY NotificationService.Repository/NotificationService.Repository.csproj NotificationService.Repository/
COPY NotificationService.Shared/NotificationService.Shared.csproj NotificationService.Shared/

# Restore
RUN dotnet restore NotificationService.API/NotificationService.API.csproj

# Copio tutto il resto
COPY . .

WORKDIR /src/NotificationService.API

RUN dotnet publish NotificationService.API.csproj \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
