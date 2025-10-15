# San Vicente Hospital - AI-Powered Appointment Management System

This C# console application is an advanced system designed to modernize and streamline the management of medical appointments, patient records, and doctor information for the San Vicente Hospital. It replaces outdated manual processes with a robust, centralized, and efficient digital solution.

A key feature of this system is the integration of a Google Gemini-powered AI, which assists patients in finding the most suitable doctor by analyzing their described symptoms, making the process of scheduling an appointment more intuitive and effective.

---

### Author Information

*   **Name:** Juan David Guzman Mendoza
*   **Clan:** Caiman
*   **Email:** Juanguzman10102005@gmail.com
*   **C.C:** 1046699456

---

### Table of Contents

1.  [Core Functionalities](#core-functionalities)
2.  [Project Structure](#project-structure)
3.  [Technologies Used](#technologies-used)
4.  [Prerequisites](#prerequisites)
5.  [Setup and Installation](#setup-and-installation)
6.  [How to Use](#how-to-use)

---

### Core Functionalities

*   **Patient Management:**
    *   Register, edit, and list patient records.
    *   Ensures data integrity by validating unique patient IDs.

*   **Doctor Management:**
    *   Register, edit, and list doctors and their specialties.
    *   Validates unique doctor IDs to prevent duplication.

*   **AI-Powered Appointment Scheduling:**
    *   Patients can describe their symptoms to an AI assistant.
    *   The AI analyzes the input, queries the hospital's database of doctors, and recommends the most appropriate specialist.
    *   Checks for scheduling conflicts to prevent double-booking for both patients and doctors.

*   **Appointment Management:**
    *   Schedule, cancel, and update the status of appointments.
    *   List appointments by patient or by doctor.

*   **Automated Email Notifications:**
    *   Automatically sends a confirmation email to the patient upon successfully scheduling an Cancel.
    *   Maintains a log of all sent emails and their delivery status ("Sent" or "Not Sent").

*   **General AI Queries:**
    *   The AI is connected to the project's database, allowing users to ask general questions about the hospital's services, doctors, or other stored information.

---

### Project Structure

The project is organized following clean architecture principles to ensure separation of concerns and maintainability.

```
/
├── Data/                 # Data storage, initialization, and context
├── Helpers/              # Utility classes and helper functions
├── Interfaces/           # C# interfaces for repositories and services
├── Menus/                # Console UI and user interaction menus
├── Models/               # C# classes representing core entities (Patient, Doctor, etc.)
├── Repositories/         # Handles data access logic (interacts with Firebase)
├── Service/              # Business logic and coordination
│   └── IA/               # Contains the AI service (GeminiService)
├── PruebaJuanGuzman.csproj # C# project file
├── PruebaJuanGuzman.sln  # Solution file
└── Program.cs            # Main entry point of the application
```

---

### Technologies Used

*   **Backend:** C# (.NET 8)
*   **Database:** Google Firebase (Firestore) for real-time data persistence.
*   **Artificial Intelligence:** Google AI Studio (Gemini API) for natural language understanding and doctor matching.
*   **Email Service:** Google App Script deployed as a web app to handle sending confirmation emails.
*   **Data Handling:** LINQ, Lists, and Dictionaries for in-memory data manipulation.

---

### Prerequisites

Before you begin, ensure you have the following installed on your system:

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
*   [Git](https://git-scm.com/downloads)
*   A Google Cloud Platform project with **Firebase (Firestore)** and the **Generative Language API** enabled.
*   A Google Account to create and deploy a **Google App Script**.

---

### Setup and Installation

Follow these steps to get the project running locally.

**Step 1: Clone the Repository**
```bash
git clone https://github.com/JuanGuzmendoza/AssetmentJuanGuzmanC-.git
cd AssetmentJuanGuzmanC
```


**Step 2: Run the Application**
```bash
dotnet run
```

---

### How to Use

Once the application is running, you will be presented with a main menu in the console.

1.  Navigate the menus using the number keys.
2.  You can choose between Administrator, Patient, or Doctor modules.
3.  **To schedule an appointment with AI assistance:**
    *   Select the **Patient** module.
    *   Choose the option to create a new appointment.
    *   When prompted, type a description of your medical problem or symptoms.
    *   The AI will process your request and suggest a suitable doctor and available time slots.
    *   Confirm your selection to book the appointment and receive an email confirmation.

---


