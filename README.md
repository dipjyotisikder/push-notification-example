# 📲 Push Notification System with Firebase and Azure

This project demonstrates a **real-world implementation** of push notification systems using both **Azure Push Notification** and **Firebase Cloud Messaging (FCM)**. It is designed to help developers integrate push notifications seamlessly into their applications.

---

## 🚀 Features

- **Azure Push Notification Integration**: Send notifications to devices using Azure Notification Hubs.
- **Firebase Cloud Messaging (FCM)**: Leverage Firebase for scalable, cross-platform notifications.
- **Firestore Database Support**: Store and retrieve notification data using Google Firestore.
- **Dependency Injection (DI)**: Clean architecture with reusable services and repositories.
- **Swagger Integration**: Explore and test APIs directly in your browser.
- **Flexible Configuration**: Easily switch between Azure and Firebase.

---

## 📦 Technologies Used

- **.NET 6** for building the backend.
- **Firebase Admin SDK** for FCM integration.
- **Google Firestore** for database operations.
- **Azure Notification Hubs** for sending notifications.
- **Swagger** for API documentation and testing.
- **Dependency Injection** for modular and testable architecture.

---

## ⚙️ Installation

### Prerequisites

1. **.NET SDK 6 or later**  
   Download and install from [here](https://dotnet.microsoft.com/).
   
2. **Google Service Account JSON**  
   Generate a service account JSON key from the Firebase console under "Project Settings > Service Accounts." Save this file for use in the app configuration.

3. **Azure Notification Hub Connection String**  
   Create a notification hub in Azure and copy the connection string.

---

### Steps to Run

1. **Clone the repository**  
   ```
   git clone push-notification-example
   cd push-notification-example
   ```

2. **Restore dependencies**  
   ```
   dotnet restore
   ```

3. **Configure the application**  
   Update the `appsettings.json` file with the following:

   - Firebase JSON Credential:
     ```json
     "GoogleCredential": {
         "type": "service_account",
         "project_id": "your-project-id",
         "private_key_id": "your-private-key-id",
         "private_key": "your-private-key",
         "client_email": "your-client-email",
         "client_id": "your-client-id",
         "auth_uri": "https://accounts.google.com/o/oauth2/auth",
         "token_uri": "https://oauth2.googleapis.com/token",
         "auth_provider_x509_cert_url": "https://www.googleapis.com/oauth2/v1/certs",
         "client_x509_cert_url": "your-cert-url"
     }
     ```

   - Azure Notification Hub:
     ```json
     "NotificationHubOptions": {
         "ConnectionString": "your-azure-connection-string",
         "HubName": "your-notification-hub-name"
     }
     ```

4. **Run the application**  
   Start the application:
   ```
   dotnet run
   ```

5. **Access the Swagger UI**  
   Navigate to [https://localhost:5001/swagger](https://localhost:5001/swagger) to explore and test the APIs.

---

## 🛠️ Usage

### Azure Push Notifications

- Inject and use the `IAzurePushNotificationService` for sending notifications via Azure Notification Hubs.
- Example usage in a controller:
   ```csharp
   public class NotificationsController : ControllerBase
   {
       private readonly IAzurePushNotificationService _azurePushNotificationService;

       public NotificationsController(IAzurePushNotificationService azurePushNotificationService)
       {
           _azurePushNotificationService = azurePushNotificationService;
       }

       [HttpPost("send-azure-notification")]
       public async Task<IActionResult> SendAzureNotification(string message, string tag)
       {
           await _azurePushNotificationService.SendNotificationAsync(message, tag);
           return Ok("Notification sent via Azure!");
       }
   }
   ```

### Firebase Push Notifications

- Store data in Firestore using the `FirebaseRepository` and send notifications via Firebase Admin SDK.
- Example usage:
   ```csharp
   public class FirebaseController : ControllerBase
   {
       private readonly IUserRepository _userRepository;

       public FirebaseController(IUserRepository userRepository)
       {
           _userRepository = userRepository;
       }

       [HttpPost("send-firebase-notification")]
       public async Task<IActionResult> SendFirebaseNotification(string userId, string message)
       {
           var user = await _userRepository.GetUserAsync(userId);
           if (user == null) return NotFound("User not found!");

           // Logic to send notification using FirebaseAdmin SDK...
           return Ok("Notification sent via Firebase!");
       }
   }
   ```

---

## 🔗 API Endpoints

### Notification Endpoints
- `POST /send-azure-notification`: Send a push notification via Azure Notification Hub.
- `POST /send-firebase-notification`: Send a push notification via Firebase.

---

## 🔑 Key Components

- **`IAzurePushNotificationService`**  
  Handles communication with Azure Notification Hub.
  
- **`IFirebaseRepository<>`**  
  Generic repository for Firestore operations.

- **`UserRepository`**  
  Custom repository to manage users in Firestore.

- **`NotificationHubOptions`**  
  Configuration for Azure Notification Hub.

---

## 📜 License

This project is licensed under the [MIT License](LICENSE).

---

## 🙌 Contributing

Contributions are welcome! If you have ideas to improve the project, feel free to fork the repository, make changes, and submit a pull request.

---

## ❤️ Acknowledgments

Special thanks to **Firebase**, **Azure**, and the **.NET Community** for their robust tools and resources that make development simpler and more effective.
