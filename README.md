# 🎟️ Event Booking System API - ASP.NET Core Web API

This is a Web API project for managing **Event Booking**, built with **ASP.NET Core**.  
The system supports **role-based access control** with 3 main roles: `User`, `Organizer`, and `Admin`, each having different permissions.

---

## 🔐 Roles & Permissions

### 👤 User
- Can view **all available events**.
- Can **register** for events.
- Can **view only their own registrations**.

### 🎤 Organizer
- Can **create events**.
- Can **view, update, and soft-delete** their own events (not permanently deleted).
- Can **restore** deleted events.

### 👑 Admin
- Can **view any user's registrations**.
- Can **soft-delete events created by organizers**.
- Can also **restore** deleted events.
📮 Postman Collection
You can test all endpoints using the shared Postman collection:

👉 [Open Postman Collection](https://www.postman.com/security-pilot-99001942/workspace/abuhussien-28/collection/26812344-218e03a2-e89b-4a66-9411-ee0d5527ffba?action=share&creator=26812344)

📌 Features (Work in Progress)
 JWT Authentication

 Role-based Authorization

 Event CRUD (Create, Read, Update, Soft Delete, Restore)

 Registration System

 Restore Deleted Events

 Email Notifications (Coming Soon)

 Admin Dashboard (Coming Soon)

📁 Technologies Used
ASP.NET Core Web API

Entity Framework Core

SQL Server

JWT Authentication

AutoMapper

Repository & Service Pattern

Postman for API Testing

🧑‍💻 Author
Ali Mohamed Ali
🟦 GitHub: Ali-Abu-Hussein
📧 Email: aliabuhussien@outlook.com

📄 License
This project is open-source and free to use.


---

## 🚀 Authentication - Login Samples

To test different roles, use the following login credentials:

### 🔹 User
```json
{
  "userName": "ali.User",
  "password": "123456"
}

```
### 🔹 Organizer

```json
{
  "userName": "ali.organizer",
  "password": "123456"
}
```

### 🔹 Admin

```json
{
  "userName": "AliMohamedAli",
  "password": "123456"
}


