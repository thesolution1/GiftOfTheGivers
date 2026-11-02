# GiftOfTheGivers

GiftOfTheGivers is an ASP.NET Core 8 web application built to support a charity/donation platform for the Gift of the Givers organization. The solution includes a web front end, database-backed data models, identity-based user management, multilingual support, and supporting tests and tooling.

## Overview

This repository contains the main application for the organization’s online presence and donation/volunteer workflows. The project is structured as a standard ASP.NET Core MVC/Razor Pages application with:

- ASP.NET Core Web App and Razor Pages
- Entity Framework Core with SQL Server
- ASP.NET Core Identity for authentication and role-based access
- Localization support for multiple languages
- MVC controllers, views, and page models for the public site
- Service and helper modules for reusable logic
- Database migrations and initial data seeding
- Testing and load-test support for validation and performance checks

## Repository structure

- `Areas/` – feature/area-based application modules
- `Controllers/` – MVC controllers for request handling
- `Models/` – data models and application entities
- `Views/` – Razor UI views
- `Pages/` – Razor Pages
- `Services/` – service-layer logic
- `GiftOfTheGivers.Helpers/` – helper utilities and shared logic
- `GiftOfTheGivers.Tests/` – automated test project
- `Migrations/` – EF Core migration files
- `wwwroot/` – static assets, CSS, JS, images
- `data/` – repository data assets
- `Properties/` – app configuration metadata
- `IdentitySeeder.cs` – seed logic for roles and initial users
- `Program.cs` – application startup, database setup, identity registration, and localization configuration

## What was done in this repository

The repo has been set up as a functional .NET web application with the following work already included:

- Created the ASP.NET Core solution and project configuration for .NET 8
- Added SQL Server database connectivity via Entity Framework Core
- Configured ASP.NET Core Identity with role-based authorization support
- Added automatic database migration and seed logic on application startup
- Included localization configuration for English, Afrikaans, and French
- Added MVC and Razor Pages routing for the app
- Added support for static files, HTTPS redirection, and authorization middleware
- Included view-model patterns for donation and volunteer page functionality
- Added supporting helper and test projects to keep the app modular and testable
- Added a load testing script using k6 for performance testing
- Included GitHub Actions workflow configuration for .NET builds

## Technology stack

- C# and ASP.NET Core 8
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- Razor Pages / MVC
- Localization / resource-based culture support
- k6 for load testing

## Configuration

The app uses configuration files such as:

- `appsettings.json`
- `appsettings.Development.json`

These hold connection strings and environment-based settings used by the application at runtime.

## Notes

The repository is a full-stack web application with both server-side logic and UI components, and it is designed to support a community-driven donation and volunteer platform with multilingual accessibility.

## Getting started

1. Restore NuGet packages
2. Configure the SQL Server connection string in `appsettings.json`
3. Run the application with `dotnet run`
4. Ensure migrations are applied automatically on startup

## Current status

This repository is structured as a working ASP.NET Core application foundation with core identity, database, localization, and web UI infrastructure already in place.
