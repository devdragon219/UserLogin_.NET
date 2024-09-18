
# APIService and MyConsoleApp Setup Guide

This guide will walk you through the process of setting up, running, and testing the **APIService** and **MyConsoleApp** projects.

## Technologies Used

- **.NET 8.0**: For both the APIService and MyConsoleApp projects.
- **ASP.NET Core**: For building the APIService.
- **Moq**: For mocking dependencies during testing.
- **xUnit**: For unit testing.
- **Newtonsoft.Json**: For handling JSON in MyConsoleApp.
- **JWT (Json Web Token)**: For authentication in APIService.
- **Identity Framework**: For managing users and authentication in APIService.

---

## Prerequisites

- **.NET SDK 8.0**: Ensure that you have .NET 8.0 installed on your machine.
- **Visual Studio 2022** or **VS Code**: Preferred for development.
- **Command Line Interface**: Powershell, Bash, or CMD.

---

## Setup

1. **Clone the repository**

2. **Navigate to APIService**

   ```bash
   cd APIService
   ```

3. **Restore Dependencies**

   Restore all the necessary packages for APIService.

   ```bash
   dotnet restore
   ```

4. **Navigate to MyConsoleApp**

   ```bash
   cd ../MyConsoleApp
   ```

5. **Restore Dependencies**

   Restore all the necessary packages for MyConsoleApp.

   ```bash
   dotnet restore
   ```

---

## Running APIService

To run the **APIService**, follow these steps:

1. **Navigate to the APIService directory**

   ```bash
   cd APIService
   ```

2. **Run the APIService**

   ```bash
   dotnet run
   ```

3. The service will be hosted at `http://localhost:5264`.

---

## Running MyConsoleApp

To run **MyConsoleApp** that interacts with the APIService:

1. **Navigate to the MyConsoleApp directory**

   ```bash
   cd ../MyConsoleApp
   ```

2. **Run the Console App**

   ```bash
   dotnet run
   ```

3. **Interact with the APIService**:
    - The app will prompt for a **username** and **password** to authenticate against the APIService.
    - Default **username**=**admin** **password**=**Admin@123**

---

## Testing APIService

To test **APIService**, unit tests have been set up using **xUnit** and **Moq** for mocking.

1. **Navigate to the APIService.Tests directory**

   ```bash
   cd ../APIService.Tests
   ```

2. **Run the tests**

   ```bash
   dotnet test
   ```

   This will execute all the unit tests and provide the results in the console.

---

## Common Issues

- **JWT Key Issues**: Make sure the JWT key in your configuration is at least 256 bits (32 characters long) to avoid key size errors during authentication.
  
- **Port Conflicts**: If you're running other services, ensure that `localhost:5264` is available, or change the port in the `launchSettings.json`.

---

## Conclusion

This setup guide helps you get started with both the APIService and MyConsoleApp projects. If you encounter any issues or need further assistance, please refer to the documentation or raise an issue in the repository.

Happy coding! 🚀
