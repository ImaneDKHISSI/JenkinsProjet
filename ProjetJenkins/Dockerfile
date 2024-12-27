# Étape 1 : Image de base pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Image pour construire le projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie uniquement les fichiers nécessaires pour restaurer les dépendances
COPY ["ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj", "ProjetJenkins/"]
WORKDIR /src/ProjetJenkins
RUN dotnet restore "ProjetJenkins.csproj"

# Copie de tout le projet et compilation
COPY . .
WORKDIR /src/ProjetJenkins
RUN dotnet publish "ProjetJenkins.csproj" -c Release -o /app/publish

# Étape 3 : Construire l'image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetJenkins.dll"]
