# 🎟️ Ticketing System API

A simplified offline ticketing Web API for organizational use. Built with ASP.NET Core, this project demonstrates role-based access control, user management, and ticket operations.

## 🚀 Features

- Role-based access control using enums
- Modular architecture with clear separation of concerns
- Swagger integration for API testing

## 🧱 Project Structure

- `Ticketing.API` – Main API project
- `Ticketing.Model` – Domain models
- `Ticketing.DTO` – Data transfer objects
- `Ticketing.Repository` – Data access layer
- `Ticketing.Service` – Business logic
- `Ticketing.ViewModel` – View models for UI/API responses
- `Ticketing.AutoMapper` – Mapping configurations
- `Ticketing.DataContext` – EF Core DbContext and migrations

## 👥 Seeded Users

To test the API, four users are seeded on first build (commented out in `Program.cs`, line 37):

| Full Name        | Email                | Role     | Password |
|------------------|----------------------|----------|----------|
| First Admin      | admin@example.com    | Admin    | string   |
| Second Admin     | admin2@example.com   | Admin    | string   |
| First Employee   | employee@example.com | Employee | string   |
| Second Employee  | employee2@example.com| Employee | string   |

## 🔐 Authorization

Role-based access is enforced via enums and a custom authorization attribute. While hardcoded access control is used here for simplicity, it’s suitable for small-scale applications and reduces complexity.

## 🛠️ Getting Started

1. Clone the repo:
   ```bash
   git clone https://github.com/jarmouzi/Ticketing.git

