✅ Challenge Title: Offline Ticketing System API for an Organization

You can run the project within the Visual Studio environment and test it using Swagger.

At the first build of the project, the Ticketing.API.Service.SeedDataAsync method seeds 4 sample users.
This seeding code is commented out at line 37 of Ticketing.API.Program.cs.

Seeded Users:

{ "FullName": "First Admin", "Email": "admin@example.com", "Password": "string", "Role": "Admin" }
{ "FullName": "Second Admin", "Email": "admin2@example.com", "Password": "string", "Role": "Admin" }
{ "FullName": "First Employee", "Email": "employee@example.com", "Password": "string", "Role": "Employee" }
{ "FullName": "Second Employee", "Email": "employee2@example.com", "Password": "string", "Role": "Employee" }

Normally, I manage role accessibility through two tables: one that records role permissions for actions, 
and another that records role permissions for data access. A custom authorization attribute enforces these 
permissions for roles and users.

In this project, I encountered a specific scenario requiring role declaration using enums. Therefore, 
I opted for a hardcoded access control approach based on enums. This approach can be suitable for small projects, 
as it simplifies implementation while reducing potential security vulnerabilities.
