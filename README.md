Study Control - Personal Learning Management Application

Overview

Study Control is a desktop-based self-learning application developed using C# WPF.

The system helps students organize and manage their independent learning activities by combining:

- PDF learning resources
- Online video learning
- Web browsing for study purposes
- Personal note management
- Study progress tracking
- Recent learning history

This application provides a centralized learning environment where users can access study materials, save notes, and monitor their learning activities.

Current Technology Stack

Desktop Application
- C#
- WPF (Windows Presentation Foundation)
- .NET Framework 4.7.2
- Material Design UI

Backend API
- ASP.NET Core Web API
- Entity Framework Core
- RESTful API architecture

Database
- Microsoft SQL Server

Additional Libraries
- Syncfusion PDF Viewer WPF
- Microsoft WebView2
- iText PDF Library
- BCrypt Password Hashing
- MailKit / MimeKit


System Architecture

Study Control follows a client-server architecture:

            User
      
             ↓
        
    WPF Desktop Application
      
            ↓
        
      ASP.NET Core Web API
      
            ↓
        
      Entity Framework Core
      
            ↓
        
      SQL Server Database

The WPF application handles the user interface and learning features, while the API manages data processing and database communication.


Major Features

User Authentication

The system provides secure user authentication.

Features:
- User registration
- Login system
- Email verification using OTP
- Password hashing with BCrypt

Authentication Flow:

    User Register
         ↓
    OTP Verification Email
         ↓
    Account Activated
         ↓
    Login Successfully


 Learning Resource Management

Users can manage different types of learning resources.

Supported resources:
- PDF documents
- Online videos
- Web resources

The application provides a single platform for accessing multiple learning materials.

PDF Learning Module

The PDF module allows users to read and manage study documents.

Features:
- Open PDF files
- View documents inside the application
- Track recently opened PDFs
- PDF history management

Technology used: Syncfusion PDF Viewer, iText PDF Library



Online Video Learning Module

The system integrates online video learning through a YouTube link, giving users the ability to watch and take notes on educational videos inside the application.

Features:
-Open and watch videos directly by entering a YouTube link
- Automatically save the video to Recent History as soon as the link is opened, using the YouTube video title and the time it was opened 
- Reopen a video instantly by selecting it from the Recent History list

Example flow:

    Enter YouTube Link
         ↓
    Video Opens & Auto-Saves to Recent(YouTube Title + Opened Time)
         ↓
    Watch Video & Take Notes
         ↓
    Click Recent Item -> Video Replays


Web Browser Module

Study Control includes an integrated browser feature. Students can:

- Search learning information
- Access educational websites
- Browse study resources without leaving the application

Note Management System

Users can create and manage personal study notes.

Features:
- Create notes
- Edit notes
- Delete notes
- Store learning information

Example flow:

    Learning Material
           ↓
    Create Note
           ↓
    Save to Database
           ↓
    Review Later

Study Progress Tracking

The system records user learning activities.

Tracked information includes:
- Recent PDF files
- Recently watched videos
- Learning activities

This helps users monitor their self-learning progress.

Database Design

The system uses SQL Server as the database.

Main tables include:
- Users
- Notes
- Tasks
- StudyProgress
- RecentPDFs
- RecentVideos

Database communication is handled through Entity Framework Core.


Security Features

Password Security

Passwords are stored using BCrypt hashing.

    User Password
         ↓
    BCrypt Hashing
         ↓
    Database Storage

Email Verification

OTP verification is used to confirm user accounts.

API Security

The application communicates with the backend through controlled API endpoints.


User Interface Design

The application uses:
- WPF UI Framework
- Material Design components

Main modules:
- Home Page
- PDF Learning
- YouTube Learning
- Browser
- Notes
- Settings
The interface is designed to provide a simple and user-friendly learning experience.


API Endpoints

The backend provides REST API services.

Main API modules:
- Auth
- Users
- Notes
- Recent PDFs
- Recent Videos

Example:

    WPF Application
         ↓
    POST /api/Auth/login
         ↓
    API Validation
         ↓
    Return User Information

Screenshots

  Login Page       [login page](login.png)
  
  Register Page    [Register Page](Register.png)
  
  Forgot Password  [Forgot Password](forgot-password.png)
  
  Home Dashboard   [Home Dashboard](home-dashboard.png)
  
  Browser Page     [Browser Page](Browser.png)
  
  PDF Page         [PDF Page](PDF.png)
                   [PDF Page](PDF-viewer.png)
                   
  Notes Page       [Note Page](Notes.png)


Development Environment

Recommended environment:
- Visual Studio 2022
- .NET Framework 4.7.2
- SQL Server
- Windows Operating System


Installation and Setup

### Requirements

Before running the project, install:
- Visual Studio 2022
- SQL Server
- .NET Framework 4.7.2 Developer Pack

### Backend Setup

1. Open the API project.
2. Update the database connection string.
3. Run database migration.
4. Start the ASP.NET Core API.

### Desktop Application Setup

1. Open the WPF project.
2. Restore NuGet packages.
3. Configure the API URL.
4. Build and run the application.

Project Structure
    
    Study Control
    │
    ├── WPF Desktop Application
    │
    ├── API Project
    │
    ├── Database
    │
    ├── Models
    │
    ├── Controllers
    │
    └── Services

Future Improvements

Planned improvements:
- User profile management
- Dark/light theme settings
- Advanced PDF annotation
- Highlight and bookmark features
- More learning analytics
- Improved study reminders

Project Purpose

Study Control was developed to provide students with an organized digital learning environment where they can access resources, manage notes, and track their independent learning activities in one application.

License

This project is licensed under the [MIT License](LICENSE).
