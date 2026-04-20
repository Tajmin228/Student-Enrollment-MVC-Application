# Student Enrollment MVC Application

A web-based Student Enrollment Management System built with **ASP.NET MVC 5** and **Entity Framework**, featuring user authentication and full CRUD operations for managing students, courses, and enrollments.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Project Structure](#project-structure)
- [Data Models](#data-models)
- [Controllers](#controllers)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Authentication](#authentication)

---

## Overview

This application allows administrators to manage student records and course enrollments. Students can be enrolled in multiple courses simultaneously. The system is protected by ASP.NET Identity authentication — most management actions require a logged-in user, while the public index pages for students and courses are accessible anonymously.

---

## Features

- **Student Management** — Create, view, edit, and delete student records including name, date of birth, age, marital status, and profile picture upload.
- **Course Management** — Full CRUD for courses with title and credit information.
- **Enrollment Management** — Assign multiple courses to a student; enrollments are updated atomically on student edit/delete.
- **User Authentication** — Registration, login, password change, two-factor authentication, and external login support via ASP.NET Identity.
- **Profile Image Upload** — Student profile pictures are saved to the `/Images/` directory on the server.
- **Authorization** — `[Authorize]` on controllers with `[AllowAnonymous]` on public listing pages.

---

## Project Structure

```
MVC_Project1294727/
│
├── Controllers/
│   ├── AccountController.cs       # Registration, login, external auth
│   ├── ManageController.cs        # Password change, phone, 2FA management
│   ├── HomeController.cs          # Home, About, Contact pages
│   ├── CoursController.cs         # Course CRUD
│   └── StudentsController.cs      # Student CRUD with enrollment handling
│
├── Models/
│   ├── Student.cs                 # Student entity
│   ├── Cours.cs                   # Course entity
│   ├── DBModels_Context.cs        # EF DbContext (StudentEnrollmentDBContext)
│   ├── DBModels.cs                # EF auto-generated models
│   └── ManageViewModels.cs        # ViewModels for account management
│
└── Views/
    ├── Cours/                     # Index, Create, Edit, Delete, Details views
    └── Enrollments/               # Index, Create, Edit, Delete, Details views
```

---

## Data Models

### Student
| Property | Type | Notes |
|---|---|---|
| `StudentID` | int | Primary Key |
| `StudentName` | string | Required |
| `BirthDate` | DateTime | Required, date format |
| `Age` | int | |
| `Picture` | string | File path to uploaded image |
| `MaritalStatus` | bool | |
| `Enrollments` | ICollection\<Enrollment\> | Navigation property |

### Cours (Course)
| Property | Type | Notes |
|---|---|---|
| `CourseID` | int | Primary Key |
| `Title` | string | |
| `Credits` | int | |
| `Enrollments` | ICollection\<Enrollment\> | Navigation property |

### Enrollment
| Property | Type | Notes |
|---|---|---|
| `EnrollmentID` | int | Primary Key |
| `StudentID` | int | Foreign Key → Student |
| `CourseID` | int | Foreign Key → Cours |
| `Grade` | string/int | Student grade for the course |

---

## Controllers

### `StudentsController`
- `GET /Students` — Lists all students with their enrolled courses (public).
- `GET/POST /Students/Create` — Creates a new student with course selections and optional image upload.
- `GET/POST /Students/Edit/{id}` — Edits student details; replaces all enrollment records.
- `GET/POST /Students/Delete/{id}` — Deletes student along with all related enrollment records.

### `CoursController`
- `GET /Cours` — Lists all courses (public).
- `GET/POST /Cours/Create` — Adds a new course (requires login).
- `GET/POST /Cours/Edit/{id}` — Edits a course (requires login).
- `GET/POST /Cours/Delete/{id}` — Deletes a course (requires login).

### `AccountController`
Handles user registration, login/logout, and external OAuth providers (Google, Facebook, etc.).

### `ManageController`
Handles authenticated user account settings: password changes, phone number verification, two-factor authentication, and linked external logins.

---

## Prerequisites

- Visual Studio 2017 or later
- .NET Framework 4.5+
- SQL Server (LocalDB or full instance)
- NuGet packages:
  - `EntityFramework`
  - `Microsoft.AspNet.Identity.EntityFramework`
  - `Microsoft.AspNet.Identity.Owin`
  - `Microsoft.Owin.Host.SystemWeb`

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MVC_Project1294727
   ```

2. **Open in Visual Studio**
   Open `MVC_Project1294727.sln`.

3. **Restore NuGet packages**
   Right-click the solution → *Restore NuGet Packages*.

4. **Configure the connection string**
   In `Web.config`, update the `StudentEnrollmentDBContext` connection string to point to your SQL Server instance:
   ```xml
   <connectionStrings>
     <add name="StudentEnrollmentDBContext"
          connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=StudentEnrollmentDB;Integrated Security=True"
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

5. **Create the Images folder**
   Ensure a folder named `Images` exists at the root of the web project (used for student photo uploads).

6. **Build and run**
   Press `F5` or click *Start* in Visual Studio.

---

## Database Setup

The project uses **Entity Framework Database-First** with a `StudentEnrollmentDBContext`. The schema is generated from the `.tt` templates (`DBModels.tt`, `DBModels_Context.tt`).

To set up the database:
1. Ensure your SQL Server is running and the connection string is correct.
2. Run the application — EF will connect to the existing database.
3. If starting fresh, create the `StudentEnrollmentDB` database manually and run any provided SQL scripts to create the `Students`, `Courses`, and `Enrollments` tables.

---

## Authentication

The application uses **ASP.NET Identity** for authentication. On first run, register a new account at `/Account/Register`. Protected routes (create, edit, delete for both students and courses) will redirect unauthenticated users to the login page.

External login providers (Google, Facebook, etc.) can be configured in `App_Start/Startup.Auth.cs`.
