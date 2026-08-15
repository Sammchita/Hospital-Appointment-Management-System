# 🏥 MediCare — Hospital Appointment Management System

MediCare is a web-based Hospital Appointment Management System developed using **ASP.NET Core MVC**. The system digitizes the appointment process and provides separate dashboards and functionalities for **Patients, Doctors, Receptionists, and Administrators**.

The project was developed to solve common problems in manual hospital appointment management, including appointment scheduling, doctor management, patient records, consultation, and prescription management.

---

## 📌 Project Overview

Traditional hospital appointment systems often rely on manual registration, phone calls, paperwork, and disconnected records.

MediCare provides a centralized system where:

- Patients can book and manage appointments.
- Doctors can view their appointments and conduct consultations.
- Doctors can create prescriptions for patients.
- Receptionists can monitor and manage daily appointments.
- Administrators can manage doctors and hospital departments.
- Role-based authorization ensures users can only access functionality appropriate to their role.

---

## ✨ Key Features

### 👤 Patient

1.  Patient registration and login
   <img width="782" height="469" alt="image" src="https://github.com/user-attachments/assets/f62b1a3d-5592-4edf-a350-4e101bc45341" />

3. Patient dashboard
   <img width="947" height="463" alt="image" src="https://github.com/user-attachments/assets/35669a8e-6f3f-43a0-bdb6-8a737e8df19d" />

  
4.  Browse hospital departments
   <img width="527" height="346" alt="image" src="https://github.com/user-attachments/assets/a46c5063-76bf-44f5-8a2b-374200868cb4" />

5.  Select doctors by department
 <img width="526" height="295" alt="image" src="https://github.com/user-attachments/assets/f75167fe-868f-4a4f-a142-af1c6a89986a" />

6.  Book appointments
   <img width="921" height="559" alt="image" src="https://github.com/user-attachments/assets/75fe5802-0f75-4a78-b539-36248a7cc0ed" />


7. Prevent duplicate doctor bookings

8. View appointment history , Cancel eligible appointments and View appointment status
   <img width="827" height="414" alt="image" src="https://github.com/user-attachments/assets/a408291d-55b9-4b52-b7ea-70a9d76dcaf2" />




### 👨‍⚕️ Doctor

- Doctor authentication
- Doctor dashboard
- View today's appointments
- View patient information
- View appointment details
- Conduct patient consultations
- Enter diagnosis
- Add prescribed medicines
- Specify dosage, frequency, duration, and instructions
- Automatically mark consulted appointments as completed

### 🧑‍💼 Receptionist

- Receptionist dashboard
- View today's appointments
- View appointment statistics
- Monitor pending appointments
- Monitor confirmed appointments
- Monitor completed appointments
- View appointment details
- View patient and doctor information

### 👨‍💻 Administrator

- Administrator authentication
- Admin dashboard
- Manage doctors
- Manage hospital departments
- View doctor information
- Manage doctor records
- Role-based access control

### 💊 Prescription Management

- Create prescriptions during consultation
- Store diagnosis
- Store multiple medicines
- Store dosage
- Store frequency
- Store duration
- Store medicine instructions
- Associate prescriptions with appointments, doctors, and patients

---

## 🔐 Role-Based Access Control

MediCare uses **ASP.NET Core Identity** and role-based authorization.

The system contains four main roles:

| Role | Main Responsibilities |
|------|------------------------|
| Patient | Book and manage appointments |
| Doctor | Manage consultations and prescriptions |
| Receptionist | Monitor and manage appointments |
| Admin | Manage doctors and departments |

###🛠️ Technologies Used
##Backend
-C#
-ASP.NET Core MVC
-ASP.NET Core Identity
-Entity Framework Core
-LINQ

##Frontend
- HTML5
- CSS3
- Razor Views
- Bootstrap
- JavaScript


## Database
- Microsoft SQL Server
- Entity Framework Core Migrations
  
## Development Tools
-Visual Studio
- SQL Server Management Studio (SSMS)
- Git
- GitHub

### Architecture

The application follows the ASP.NET Core MVC architecture.

                    ┌─────────────────────┐
                    │       Browser       │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │    MVC Controllers  │
                    └──────────┬──────────┘
                               │
                 ┌─────────────┼─────────────┐
                 ▼             ▼             ▼
             Models       ViewModels       Views
                 │             │             │
                 └─────────────┼─────────────┘
                               ▼
                    ┌─────────────────────┐
                    │ Entity Framework    │
                    │       Core          │
                    └──────────┬──────────┘
                               │
                               ▼
                    ┌─────────────────────┐
                    │    SQL Server DB    │
                    └─────────────────────┘
