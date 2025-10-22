# CosmosAflevering

#  SupportApp – Cosmos DB & Blazor Projekt

##  Projektbeskrivelse
**SupportApp** er en webapplikation udviklet i **ASP.NET Core Blazor Server**, som gør det muligt for brugere at oprette og hente supporthenvendelser.  
Applikationen gemmer data i **Azure Cosmos DB** og viser dem direkte i brugergrænsefladen.  
Formålet er at demonstrere integration mellem et Blazor .NET-projekt og en cloud-baseret NoSQL-database.

---

##  Teknologistak
| Komponent | Teknologi |
|------------|------------|
| Frontend | Blazor Server (.NET 7) |
| Backend | C# (.NET Core) |
| Database | Azure Cosmos DB (SQL API) |
| Hosting / Cloud | Microsoft Azure |
| Dataformat | JSON |
| IDE | Visual Studio 2022 |
| Versionsstyring | Git & GitHub |

---

##  Projektstruktur

SupportApp/
│
├── Pages/
│ ├── CreateSupport.razor # Form til oprettelse af supporthenvendelser
│ ├── FetchSupport.razor # Viser alle supporthenvendelser
│
├── Services/
│ ├── ISupportService.cs # Interface for datatjeneste
│ ├── SupportService.cs # Implementering med Cosmos DB-integration
│
├── Models/
│ ├── SupportMessage.cs # Datamodel for en henvendelse
│
├── wwwroot/ # CSS, JS og statiske filer
│
├── appsettings.json # Indeholder Cosmos DB connection string og database-info
├── Program.cs # Konfiguration af app og dependency injection
├── README.md # Dokumentation (denne fil)
└── SupportApp.sln # Solution-fil

---

##  Cosmos DB Konfiguration
Applikationen bruger **Azure Cosmos DB** med følgende opsætning:

| Indstilling | Værdi |
|--------------|--------|
| Database ID | `IBasSupportDB` |
| Container ID | `ibassupport` |
| Partition Key | `/category` |
| API-type | Core (SQL) |

### Eksempel på dokumentstruktur i Cosmos DB:
json
{
    "id": "001",
    "category": "Teknisk spørgsmål",
    "description": "Min elcykel viser fejl E04, kan I hjælpe med at nulstille systemet?",
    "date": "2025-10-11T09:15:00Z",
    "customer": {
        "name": "Mette Larsen",
        "email": "mette.larsen@example.com",
        "phone": "+45 22446688"
    }
}


## Funktionalitet 

🔹 1. Opret supporthenvendelse

Brugeren udfylder navn, email, telefon, kategori og beskrivelse.

Data sendes til Cosmos DB som et JSON-dokument.

SupportService håndterer forbindelsen via CosmosClient.

🔹 2. Hent alle henvendelser

Henvendelser vises i en tabel sorteret efter dato.

Der er søgefelt til filtrering på navn eller email.

Data hentes via GetAllSupportMessagesAsync() fra SupportService.



## Centrale kodekomponenter

SupportService.cs

Ansvarlig for al kommunikation med Cosmos DB:

public async Task AddSupportMessageAsync(SupportMessage message)
{
    message.Id ??= Guid.NewGuid().ToString();
    message.CreatedAt = DateTime.UtcNow;
    await _container.CreateItemAsync(message, new PartitionKey(message.PartitionKey));
}



SupportMessage.cs

Model, der afspejler strukturen i Cosmos DB:

public class SupportMessage
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonProperty("category")]
    public string Subject { get; set; } = string.Empty;

    [JsonProperty("description")]
    public string Message { get; set; } = string.Empty;

    [JsonProperty("date")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [JsonProperty("customer")]
    public CustomerInfo Customer { get; set; } = new();

    [JsonIgnore]
    public string PartitionKey => Customer.Email ?? "default";
}



## Sådan kører du projektet lokalt

1. Sørg for, at du har .NET SDK 7.0 eller nyere installeret.

2. Åbn projektet i Visual Studio eller kør fra terminalen:

dotnet run

3. Applikationen starter på:

http://localhost:5264

4.Opret nogle henvendelser og se dem derefter på “Fetch Support”-siden.



| Krav                                | Status | Kommentar                                |
| ----------------------------------- | ------ | ---------------------------------------- |
| CRUD-funktionalitet (Create + Read) | ✅      | Opret og hent supporthenvendelser        |
| Azure Cosmos DB integration         | ✅      | Via `CosmosClient` og JSON-struktur      |
| Data vises i Blazor UI              | ✅      | Tabel med filtreringsfunktion            |
| Brugerinput valideres               | ✅      | DataAnnotations-validatorer i `EditForm` |
| README.md dokumentation             | ✅      | Denne fil                                |





## Opret en Cosmos DB Database med Azure CLI

For at projektet kan køre, skal du have en Azure Cosmos DB-database med korrekt struktur.
Her er et eksempel på, hvordan du kan oprette den via Azure CLI:

# Log ind i Azure
az login

# Opret en resource group
az group create --name SupportRG --location UK South

# Opret en Cosmos DB konto
az cosmosdb create --name sonderlin --resource-group SupportRG --kind GlobalDocumentDB

# Opret database
az cosmosdb sql database create --account-name sonderlin --resource-group SupportRG --name IBasSupportDB

# Opret container (med partition key /category)
az cosmosdb sql container create \
  --account-name sonderlin \
  --resource-group SupportRG \
  --database-name IBasSupportDB \
  --name ibassupport \
  --partition-key-path "/category"

  Når det er gjort, kan du finde din Connection String og Primary Key i Azure Portal →
Cosmos DB → Keys og indsætte dem i appsettings.json.


## Hvad jeg har nået

Fuldt fungerende Blazor app, der kan:

Oprette henvendelser via formular

Gemme dem i Cosmos DB

Hente og vise dem i en tabel

Cosmos DB integration via CosmosClient

JSON-model der matcher database-strukturen



## Hvad der mangler

Lidt ekstra validering i formularen 

Forbedret design/styling

Evt. opdatering/sletning af henvendelser



## Hvad vi ville lave som næste trin

Implementere login-system (Azure AD eller Identity)

Tilføje filtrering/søgning i Cosmos DB

Migrere til .NET 8 for bedre performance og support






## Udviklet af

Navn: Christian Sonderlin Busk
Dato: 22. oktober 2025
Fag: ITA-E24A – Cloud-computing - Cosmos DB aflevering