# GlamBook — Online Beauty Salon Booking System

A full-stack web application for booking appointments at beauty salons, built with ASP.NET Core MVC and Clean Architecture principles.


##  About The Project

GlamBook is a modern web platform that replaces the traditional phone-based booking system in beauty salons. Users can browse salons by category, view services with prices and duration, and book appointments through an interactive calendar that automatically blocks already-taken time slots.

The system supports three user roles with different levels of access — regular users who book appointments, managers who manage their salon's bookings, and administrators who have full control over the platform.

---

##  Features

###  User
- Register and login
- Browse salons by category
- View services with price and duration
- Book appointments via interactive calendar
- Blocked time slots for already-booked appointments
- View and cancel own appointments
- Automatic email notifications

###  Manager
- View appointments for their salon only
- Confirm and cancel bookings
- Manage salon services
- Filter appointments by status, date, and customer email

###  Admin
- Full salon management (create, edit, delete)
- Category management
- Assign managers to salons
- View and manage all appointments across all salons
- Filter appointments by salon, status, date, and customer email

---

## 🛠️ Built With

### Backend
| Technology | Purpose |
|---|---|
| ASP.NET Core MVC (.NET 8) | Web framework |
| C# | Programming language |
| Entity Framework Core | ORM / Database access |
| Microsoft SQL Server | Database |
| ASP.NET Core Identity | Authentication & Authorization |
| AutoMapper | Entity ↔ DTO mapping |
| MailKit | Email notifications via Gmail SMTP |

### Frontend
| Technology | Purpose |
|---|---|
| HTML5 & CSS3 | Structure and styling |
| JavaScript | Interactive calendar, filters |
| Google Fonts | Typography (Cormorant Garamond, Montserrat) |

### Tools & Architecture
| Tool | Purpose |
|---|---|
| Visual Studio 2022 | IDE |
| Clean Architecture | Project structure |
| Azure App Service | Hosting (planned) |
| Azure SQL Database | Production database (planned) |

---

## Architecture

The project follows **Clean Architecture** principles, organized into 4 layers:

```
┌─────────────────────────────────────────┐
│         GlamBook.Web                    │
│  Controllers · Razor Views · Areas      │
│  (Admin, Manager) · Identity UI         │
├─────────────────────────────────────────┤
│         GlamBook.Application            │
│  Interfaces · DTOs · AutoMapper         │
├─────────────────────────────────────────┤
│         GlamBook.Infrastructure         │
│  Services · EF Core · EmailService      │
├─────────────────────────────────────────┤
│         GlamBook.Domain                 │
│  Entities · Enums · BaseEntity          │
└─────────────────────────────────────────┘
          ↕ External Services
   [SQL Server]        [Gmail SMTP]
```


Each layer depends only on the layer below it — Domain has no external dependencies.

---

##  Database Schema

| Entity | Description |
|---|---|
| AppUser | Extends IdentityUser, manages bookings |
| Salon | Beauty salon with manager assignment |
| Category | Service categories (Hair, Face, Body) |
| Service | Salon services with price and duration |
| Appointment | Booking linking User, Salon and Service |

---

##  Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (LocalDB or full)
- Visual Studio 2022

### Installation

1. Clone the repository
```bash
git clone https://github.com/yourusername/glambook.git
```

2. Navigate to the project
```bash
cd glambook
```

3. Update the connection string in `appsettings.json`
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GlamBookDb;Trusted_Connection=True;"
}
```

4. Add your email settings in `appsettings.json`
```json
"EmailSettings": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "SenderEmail": "your-email@gmail.com",
  "SenderName": "GlamBook",
  "Password": "your-app-password"
}
```

5. Apply database migrations
```bash
dotnet ef database update
```

6. Run the application
```bash
dotnet run
```

---

##  Default Roles

The application seeds three roles on startup:
- **Admin** — Full system access
- **Manager** — Salon-specific access
- **User** — Standard booking access

---

##  Email Notifications

Automatic emails are sent to users when:
- ✅ Appointment is confirmed by manager
- ❌ Appointment is cancelled by manager or admin
- 🚫 User cancels their own appointment

---
## Project Structure

```
GlamBook/
├── GlamBook.Domain/
│   ├── Entities/
│   │   ├── AppUser.cs
│   │   ├── Salon.cs
│   │   ├── Service.cs
│   │   ├── Category.cs
│   │   └── Appointment.cs
│   ├── Enums/
│   │   └── AppointmentStatus.cs
│   └── Common/
│       └── BaseEntity.cs
├── GlamBook.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Mappings/
├── GlamBook.Infrastructure/
│   ├── Data/
│   ├── Services/
│   └── Migrations/
└── GlamBook.Web/
    ├── Controllers/
    ├── Views/
    └── Areas/
        ├── Admin/
        └── Manager/
```
---

##  Built As

This project was developed as a graduation thesis at the Faculty of Computer Science and Engineering (FINKI), Ss. Cyril and Methodius University, Skopje — 2026.

---

##  License

This project is for academic purposes.
