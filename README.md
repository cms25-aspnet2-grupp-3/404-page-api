# 404 Page Monitoring API

Ett backend-microservice byggt med ASP.NET Core Web API som loggar besök på 404-sidor från en Next.js-applikation.  
Tjänsten samlar in information om ogiltiga URL:er och sparar metadata för analys och övervakning.

---

## 🚀 Live API

Swagger-dokumentation finns här:

👉 https://shiko404page.azurewebsites.net/swagger/index.html

---

## 📌 Syfte

Denna tjänst är en del av ett större LMS-projekt och ansvarar för:

- Loggning av trasiga eller ogiltiga URL:er (404-sidor)
- Spårning av användarbeteende vid felaktiga sidförfrågningar
- Insamling av data för analys och övervakning
- Demonstration av microservice-arkitektur tillsammans med frontend

---

## 🧱 Teknikstack

- ASP.NET Core Web API (.NET 8)
- Entity Framework Core
- SQL Server (LocalDB / Azure SQL)
- Swagger / OpenAPI
- Azure App Service
- CORS konfigurerat för Next.js frontend

---

## 🔗 Integration med frontend

Tjänsten används av en Next.js-applikation som:

- Skickar en POST-request när en användare hamnar på en 404-sida
- Samlar in:
  - Ogiltig URL
  - Referrer
  - User agent
  - Timestamp (server-side)
  - IP-adress (server-side)

---

### Exempel på endpoint

`POST /api/monitoring/log`

### Headers

# ```txt
x-api-key: din-api-nyckel
Content-Type: application/json
Request body
{
  "invalidUrl": "http://localhost:3000/missing-page",
  "referrer": "http://localhost:3000/dashboard",
  "userAgent": "Mozilla/5.0 ..."
}
📊 Tillgängliga endpoints
POST /api/monitoring/log

Loggar en 404-händelse.

Säkerhet:

Kräver API-nyckel via header (x-api-key)
GET /api/monitoring/logs

Returnerar de senaste 50 loggade 404-händelserna.

🔐 Säkerhet
API-nyckel krävs för skrivoperationer
CORS begränsad till frontend-origin
Validering sker via model binding i ASP.NET Core
Data lagras via Entity Framework Core
☁️ Deployment

Tjänsten är deployad via Azure App Service:

Backend: Azure App Service
Databas: SQL Server (LocalDB i utveckling / Azure SQL i produktion)
Swagger aktiverat för test och demonstration
🧪 Tester

Testning är implementerad med:

xUnit
InMemory EF Core databas
Mockad konfiguration och HTTP context
Tester täcker:
Validering av API-nyckel
Lyckad loggning
Databaslagring
📁 Projektstruktur
Controllers/
Data/
Models/
Migrations/
Program.cs
ApiDbContext.cs
👨‍💻 Utvecklare

Grupparbete – Nackademin CMS25
ASP.NET Core + Next.js microservice-projekt
