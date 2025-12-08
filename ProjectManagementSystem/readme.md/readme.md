# Project Management System

## 1. Project Overview

**Short Description:**  
This project is a console-based Project Management System. It allows project managers and team members to organize
their work efficiently by assigning tasks, setting deadlines, and tracking progress. All data is
saved persistently in a MySQL database.

**Target Users:**
- Project Managers
- Team Members

**Core Features:**
- Project Managers can create, edit, and assign tasks with deadlines.
- Team Members can view, accept, complete, and report tasks.
- Task statuses are updated dynamically using strategies.
- All user and task data is stored in a MySQL database for persistence.

**Assumptions / Constraints:**
- Have only used JetBrains Rider to execute this project.
- Console-based UI only.
- Users must have a role assigned at creation (Project Manager or Team Member).
- MySQL with the correct schema and tables are needed for this project to run.

---

## 2. Build & Run Instructions

1. **MySql Setup**
- If not already installed, download MySQL Workbench 8.0 CE
  - link: https://dev.mysql.com/downloads/workbench/8.0.html
- Create a connection and enter:
  - Connection Name: Local MySQL
  - Hostname: localhost
  - Username: root (or another user you created)
  - Password: your MySQL password
- Inside the connection right-click the schemas sidebar and select Create Schema
- Name the schema projectmanagementsystemdb and click apply twice and finish.
- Create and run this SQL Script:

USE projectmanagementsystemdb;
CREATE TABLE IF NOT EXISTS Users (
UserId INT AUTO_INCREMENT PRIMARY KEY,
FirstName VARCHAR(50) NOT NULL,
LastName VARCHAR(50) NOT NULL,
Email VARCHAR(150) NOT NULL UNIQUE,
Password VARCHAR(255) NOT NULL,
Role ENUM('ProjectManager', 'TeamMember'));

CREATE TABLE IF NOT EXISTS Tasks (
TaskId INT AUTO_INCREMENT PRIMARY KEY,
Title VARCHAR(50) NOT NULL,
Description VARCHAR(255),
AssignedTo INT,
AssignedBy INT,
Status ENUM('Unassigned','Assigned','InProgress','Complete','Reported','Approved') ,
Deadline DATE,
TaskType Enum('Standard', 'Urgent'),
FOREIGN KEY (AssignedTo) REFERENCES Users(UserId) on DELETE SET NULL,
FOREIGN KEY (AssignedBy) REFERENCES Users(UserId) on DELETE SET NULL);
INSERT INTO Users (FirstName, LastName, Email, Password, Role) VALUES
('John', 'Doe', 'johndoe@gmail.com', 'test123', 'ProjectManager'),
('Alice', 'Johnson', 'alice.johnson@gmail.com', 'pass123', 'TeamMember'),
('Bob', 'Smith', 'bob.smith@gmail.com', 'pass123', 'TeamMember'),
('Carol', 'Lee', 'carol.lee@gmail.com', 'pass123', 'TeamMember'),
('David', 'Brown', 'david.brown@gmail.com', 'test123', 'ProjectManager');
INSERT INTO Tasks (Title, Description, AssignedBy, AssignedTo, Status, Deadline, TaskType) VALUES
('Setup Project Repo', 'Initialize GitHub repo for the project', 1, 2, 'Assigned', '2026-1-10', 'Standard'),
('Design Database Schema', 'Design and MySQL schema', 1, 3, 'InProgress', '2025-12-12', 'Urgent'),
('Create UI Mockups', 'Create React UI mockups for the dashboard', 1, 4, 'Unassigned', '2026-3-1', 'Standard'),
('Write Unit Tests', 'Write tests for backend APIs', 5, 4, 'Assigned', '2025-12-30', 'Standard'),
('Deploy', 'Deploy frontend to production', 5, 2, 'Unassigned', '2025-12-10', 'Urgent');

2. **Environment Setup**
**Tools, Versions, and Packages:**
- JetBrainsRider: 
  - Target Framework: .NET SDK 9.0
- Use NuGet to install required packages
  - If it shows 0 available packages copy and paste this into your terminal:
  - dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
- Required Packages:
  - MySql.Data v9.5.0
  - DotNetEnv v3.1.1
- Create a .env file in the project root folder (next to the src folder and other directories).
- Enter your connection string as such:
  - DB_CONNECTION_STRING="Server=localhost;Database=ProjectManagementSystemDB;User ID=root;Password={password}"
  - {password} = your mysql connection password from earlier

**Now use Ctrl+f5 or click the arrow button to run the app.**
- Note: Since you can see the passwords in the database you can log in to different users to test different roles or you can just register instead
- Once you are logged into the system, based on your you will be able to:
  - Create tasks
  - Assign tasks
  - Mark tasks as different statuses
  - etc.
## 3. Required OOP Features

| OOP Feature / Pattern      | File Name                          | Line Numbers | Reasoning / Purpose                                                                                                  |
|----------------------------|------------------------------------|--------------|----------------------------------------------------------------------------------------------------------------------|
| Inheritance Example 1      | /Models/User.cs                    | All          | `ProjectManager` and `TeamMember` inherit from `User` to share common attributes and behavior.                       |
| Inheritance Example 2      | /Models/ProjectTask.cs             | All          | `UrgentTask` and `StandardTask` inherit from `ProjectTask` to define specialized task types.                         |
| Interface Implementation 1 | /Repositories/UserRepository.cs    | All          | Implements `IUserRepository` to define repository contract for user operations for reading, adding, and deleting.    |
| Interface Implementation 2 | /Repositories/ProjectTaskService.cs | All          | Implements `IProjectTaskService` to define task management operations.                                               |
| Interface Implementation 3 | /Services/UserService.cs           | All          | Implements `IUserService` interface for user-related operations.                                                     |
| Polymorphism Example 1     | /Factories/ProjectTaskFactory.cs   | All          | `CreateTask` method returns different task subclasses (`StandardTask` or `UrgentTask`) but handled as `ProjectTask`. |
| Polymorphism Example 2     | /Factories/TaskDisplayer.cs        | All          | Uses `ITaskDisplayStrategy` to display tasks differently based on type.                                              |
| Struct                     | /Models/Deadline.cs                | All          | Represents a task’s due date and provides a simple way to check if it’s overdue.                                     |
| Enum                       | /Enums/ProjectTaskStatus.cs        | All          | Represents task status (`Unassigned`, `Assigned`, `InProgress`, `Complete`, `Reported`, `Approved`).                             |
| Singleton                  | /Services/Database.cs              | All          | Ensures only one MySQL database connection exists.                                                                   |
| Factory Method Example     | /Factories/UserFactory.cs          | All          | Creates users or tasks dynamically based on role/type.                                                               |
| Strategy Example           | /Strategies/TaskDisplayer.cs       | All          | Handles task display behaviors dynamically without changing core logic.                                              |
| Data Structure Example     | /Services/ProjectTaskService.cs    | 42–56        | Uses `List<ProjectTask>` to store and be used for filtering out incomplete tasks.                                    |
| I/O                        | /Menu/TeamMemberMenu.cs            | All          | Console UI with user input/output specifically for Team Members                                                      |

**Access Modifiers Explanation** 

| Access Modifier         | Explanation                                                                                                          | Example                                                                                                                                                                                             |
|-------------------------|----------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Private Method          | Some key classes have private methods that are used for internal logic that other classes should not have access to. | `/Menu/TeamMemberMenu.cs` has several private methods that act as helpers to keep code clean and do operations such as marking a task.                                                              |
| Private Field           | Dependencies that are injected into the class                                                                        | `/Services/UserService.cs` Has two private fields, IUserFactory _userFactory and IUserRepository _userRepository. On creation these two will be injected into the class and will not work otherwise |
| Public Method and Field | Used so that other classes can access it.                                                                            | `/Repository/UserRepository.cs` has several public methods like Add() which lets UserService and other classes interact with the repository.                                                     |





## 4. Design Patterns
| Pattern Name   | Category   | File Name                          | Line Numbers | Rationale                                                                                                      |
| -------------- | ---------- |------------------------------------|--------------| -------------------------------------------------------------------------------------------------------------- |
| Singleton      | Creational | /Services/Database.cs              | All          | Ensures only one database connection exists for the application.                                               |
| Factory Method | Creational | /Factories/UserFactory.cs          | All          | Creates `ProjectManager` or `TeamMember` objects dynamically based on role. Centralizes object creation logic. |
| Strategy       | Behavioral | TaskDisplayer.cs                   | All          | Allows different display behaviors for tasks (Standard vs Urgent) without changing core display logic.         |
| Strategy       | Behavioral | /Strategies/TaskStatusProcessor.cs | All          | Handles task status changes dynamically (Accept, Complete, RequestRevision) without modifying core logic.      |


## 5. Design Decisions
 
**Main Components:**  

- System.cs: Entry point and console menu controller.
- AuthenticationController registers or logs the user into the system.
- MenuFactory.cs: Creates menus dynamically based off of user role.
- ProjectTask and User Services create app interactivity, acting as a medium to manipulate corresponding repositories.
- ProjectTask and User Repositories persistently hold corresponding data.
- TaskDisplayer.cs outputs tasks differently to the console based off of their type.

- TaskDisplayer: Displays tasks using Strategy pattern.

- Repositories (UserRepository, ProjectTaskRepository): Handles database operations.

- Factories: Creates users, tasks, and menu displays dynamically.

**Key Abstractions & Tradeoffs**:

- Interfaces (like IProjectTaskRepository and ITaskDisplayer) allow for decoupling. 

- Console-based UI chosen for simplicity; future extension could include web or GUI frontend.