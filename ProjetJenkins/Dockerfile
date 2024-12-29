# Étape 1 : Image de base pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Image pour construire le projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copie des fichiers nécessaires pour restaurer les dépendances
COPY ["ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj", "ProjetJenkins/"]
COPY ["ProjetJenkins/TestUnitaire/TestUnitaire.csproj", "TestUnitaire/"]

WORKDIR /src/ProjetJenkins

# Restauration des dépendances
RUN dotnet restore "ProjetJenkins.csproj"
RUN dotnet restore "TestUnitaire/TestUnitaire.csproj"

# Copie de tout le projet et compilation
COPY . .
WORKDIR /src/ProjetJenkins

# Exécute les tests
RUN dotnet test TestUnitaire/TestUnitaire.csproj --no-build

# Publier le projet
RUN dotnet publish "ProjetJenkins.csproj" -c Release -o /app/publish

# Étape 3 : Construire l'image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetJenkins.dll"]
