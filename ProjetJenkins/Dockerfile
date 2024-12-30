# Étape 1 : Image de base pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Image pour construire le projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier le fichier .csproj principal
COPY ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj ProjetJenkins/ProjetJenkins/

# Copier le fichier .csproj des tests unitaires
COPY ProjetJenkins/TestUnitaire/TestUnitaire.csproj ProjetJenkins/TestUnitaire/

# Vérification des fichiers copiés
RUN ls -l ProjetJenkins/ProjetJenkins
RUN ls -l ProjetJenkins/TestUnitaire

# Nettoyage conditionnel
RUN [ -f ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj ] && dotnet clean ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj || echo "ProjetJenkins.csproj not found"

# Restauration des dépendances
RUN dotnet restore ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj

# Copier tout le contenu de JenkinsProjet dans /src
COPY . .

# Construire le projet principal
RUN dotnet build ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj -c Release --no-restore

# Exécuter les tests unitaires
RUN dotnet test ProjetJenkins/TestUnitaire/TestUnitaire.csproj --no-build

# Publier le projet principal
RUN dotnet publish ProjetJenkins/ProjetJenkins/ProjetJenkins.csproj -c Release -o /app/publish

# Étape 3 : Construire l'image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetJenkins.dll"]
