using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Welcome to My Console App that integrates with APIService!");

            Console.WriteLine("Please enter your username:");
            string username = Console.ReadLine();

            Console.WriteLine("Please enter your password:");
            string password = ReadPassword();

            // Prepare the login request
            var loginRequest = new
            {
                Username = username,
                Password = password
            };

            // Serialize the login request to JSON
            string jsonRequest = JsonConvert.SerializeObject(loginRequest);

            // Send the request to the API
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5264"); // Change the port if needed

            try
            {
                HttpContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("/api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<LoginResponse>(resultContent);

                    Console.WriteLine($"Login Success! Your token is: {result.Token}");

                    // Display the full user profile
                    Console.WriteLine("User Profile:");
                    Console.WriteLine($"Username: {result.Profile.UserName}");
                    Console.WriteLine($"Email: {result.Profile.Email}");
                    Console.WriteLine($"Phone: {result.Profile.PhoneNumber}");

                    // Notification: If login is successful, display success message
                    ShowNotification("Login Success", true);
                }
                else
                {
                    var resultContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(resultContent);
                    Console.WriteLine($"Login failed. Reason: {errorResponse.Message}");

                    // Notification: If login fails, display error message
                    ShowNotification(errorResponse.Message, false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Caught!");
                Console.WriteLine($"Message :{ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ShowNotification(string message, bool isSuccess)
        {
            if (isSuccess)
            {
                Console.WriteLine($"Notification: {message}");
            }
            else
            {
                Console.WriteLine($"Notification: Error - {message}");
            }
        }

        // Function to read password input from the console without displaying it
        private static string ReadPassword()
        {
            StringBuilder password = new StringBuilder();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password.Append(key.KeyChar);
                    Console.Write("*"); // Mask the password characters with '*'
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b"); // Erase the last '*' when backspace is pressed
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password.ToString();
        }
    }

    // Class for deserializing the response from the API
    public class LoginResponse
    {
        public string Token { get; set; }
        public UserProfile Profile { get; set; }
    }

    // Class for deserializing the user profile
    public class UserProfile
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    // Class for handling error responses
    public class ErrorResponse
    {
        public string Message { get; set; }
    }
}
