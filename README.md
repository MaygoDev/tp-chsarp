# CarDealer

Application console pour gérer clients, véhicules et achats d'un concessionnaire automobile.

## Fonctionnalités principales
- Import et lecture de fichiers CSV d'initialisation (`Data/clients.csv`, `Data/voitures.csv`).
- Gestion des entités : `Client`, `Vehicle`, `Purchase`.
- Persistance via Entity Framework Core (migrations dans `Migrations/`).
- Interface en ligne de commande (implémentée dans `Cli/CarDealerCli.cs`) pour lister, ajouter et gérer les enregistrements.
- Utilitaires de formatage et variables globales dans `Utils/`.

## Structure du projet (points importants)
- `Program.cs` — point d'entrée.
- `CarDealerDbContext.cs` et `Database.cs` — configuration et accès à la base.
- `CSVReader.cs` — import CSV.
- Dossiers : `Models/`, `Data/`, `Migrations/`, `Cli/`, `Utils/`.

## Prérequis
- .NET 9 SDK installé.
- (Optionnel) Outils EF Core si vous exécutez manuellement les migrations.

## Comment démarrer (Windows)
1. Ouvrir un terminal dans le dossier du projet.
2. Builder :
   ````bash
   dotnet build
    ````
3. Lancer :
   ````bash
    dotnet run --project CarDealer.csproj
   ```
ou exécuter le binaire :
   ```
bin\Debug\net9.0\CarDealer.exe
   ```

## Base de données et migrations
- Les migrations EF Core sont dans `Migrations/`. L'application applique normalement les migrations au démarrage ; sinon :
  ```
    dotnet ef database update
  ```
  (nécessite les outils EF Core installés).

## Usage rapide
- Utiliser la CLI pour :
  - lister les clients/véhicules/achats,
  - ajouter un client ou un véhicule,
  - enregistrer un achat.
- Voir `Cli/CarDealerCli.cs` pour les commandes disponibles.
