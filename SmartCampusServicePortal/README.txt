Smart Campus Service Portal - ASP.NET Core MVC + EF Core (SQL Server) + REST API

REQUIREMENTS
- Visual Studio 2022 with the "ASP.NET and web development" workload
- .NET 8 SDK (installed with VS 2022 17.8+)  [newer runtimes also work]
- SQL Server running locally (same one you used for the other labs)
- Internet connection (Bootstrap / icons / jQuery load from CDN)

RUN
1. EXTRACT the zip. Double-click SmartCampusServicePortal.sln.
2. Press F5 (https://localhost:44335).
   The database "SmartCampusDB" and the sample data (4 students, 4 requests)
   are created automatically on first run. No migrations needed.

CONNECTION STRING (SmartCampusServicePortal\appsettings.json)
  Default SQL Server :  Server=localhost;Database=SmartCampusDB;Trusted_Connection=True;TrustServerCertificate=True;
  SQL Express        :  Server=.\SQLEXPRESS;...
  LocalDB            :  Server=(localdb)\MSSQLLocalDB;...

PAGES
  /                         Dashboard
  /ServiceRequests          List / Details / Create / Edit / Delete / Track
  /Admin                    Filter + Change Status
  /Students                 Directory / Register / Profile / Portal
  /api/servicerequests      REST API (GET, GET/{id}, POST, PUT/{id}, PUT/{id}/status, DELETE/{id})
