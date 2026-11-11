#Etapa 1 - Builder
FROM mcr.microsoft.com/dotnet/sdk:10.0 as builder
WORKDIR /usr/src/app
COPY /GestionClinicaNutricionalService .
RUN dotnet restore .
RUN dotnet publish . -c Release --no-restore -o publish

#Etapa 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /usr/share/app
copy --from=builder /usr/src/app/publish /usr/share/app

EXPOSE 80
ENV ASPNETCORE_URLS=http://*:80
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
ENTRYPOINT ["dotnet", "GestionClinicaNutricionalService.WebApi.dll"]