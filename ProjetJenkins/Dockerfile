# Étape 1 : Image de base pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Étape 2 : Image pour construire le projet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers .csproj
COPY ProjetJenkins/ProjetJenkins.csproj ProjetJenkins/

# Restaurer les dépendances
RUN dotnet restore ProjetJenkins/ProjetJenkins.csproj

# Copier tout le contenu du projet
COPY . .

# Construire le projet principal
RUN dotnet build ProjetJenkins/ProjetJenkins.csproj -c Release --no-restore

# Publier le projet principal
RUN dotnet publish ProjetJenkins/ProjetJenkins.csproj -c Release -o /app/publish

# Étape 3 : Construire l'image finale
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjetJenkins.dll"]
