using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Hospital.Models;
using Hospital.Repositories;
using Helpers;

namespace Hospital.Services
{
    /// <summary>
    /// Service for managing and sending appointment-related emails to patients.
    /// </summary>
    public static class EmailService
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly EmailRepository _emailLogRepo = new();
        private static readonly string AppsScriptUrl = "https://script.google.com/macros/s/AKfycbweQdP0TxXh02tmmULqEgXuhmFWCaClrdWmLCXHODhGe3xOwf4-lDjyJwjjaCsXg_PeLg/exec";

        static EmailService()
        {
            // Set the security protocol depending on the operating system
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
            else
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            }
        }


        /// <summary>
        /// Sends an appointment-related email to a patient.
        /// The content of the email depends on whether it's a confirmation or cancellation.
        /// </summary>
        /// <param name="appointment">The appointment details.</param>
        /// <param name="patientEmail">The email address of the patient.</param>
        /// <param name="selectedDoctor">The doctor associated with the appointment.</param>
        /// <param name="action">The type of email action (Confirmation or Cancellation).</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SendAppointmentEmailAsync(Appointment appointment, string patientEmail, Doctor selectedDoctor, EmailAction action)
        {
            // Define the subject and body of the email based on the action
            string subject = action == EmailAction.Confirmation ? "Medical Appointment Confirmation" : "Medical Appointment Cancellation";

            string emailBody = action == EmailAction.Confirmation
                ? $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f9;
                                    color: #333;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: 0 auto;
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 8px;
                                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                }}
                                h2 {{
                                    color: #0044cc;
                                }}
                                p {{
                                    font-size: 16px;
                                    line-height: 1.6;
                                }}
                                .footer {{
                                    font-size: 12px;
                                    text-align: center;
                                    color: #777;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <h2>Medical Appointment Confirmation</h2>
                                <p>Dear patient,</p>
                                <p>Your medical appointment with Dr. {selectedDoctor.Name} has been confirmed for <strong>{appointment.AppointmentDate:dddd, dd MMMM yyyy HH:mm}</strong>.</p>
                                <p>Please arrive 15 minutes before the scheduled time. If you are unable to attend, please contact us as soon as possible to reschedule.</p>
                                <p>Thank you for choosing San Vicente Hospital.</p>
                                <p class='footer'>This is an automated message. Do not reply to this email.</p>
                            </div>
                        </body>
                    </html>"
                : $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: Arial, sans-serif;
                                    background-color: #f4f4f9;
                                    color: #333;
                                }}
                                .container {{
                                    max-width: 600px;
                                    margin: 0 auto;
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 8px;
                                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                                }}
                                h2 {{
                                    color: #cc0000;
                                }}
                                p {{
                                    font-size: 16px;
                                    line-height: 1.6;
                                }}
                                .footer {{
                                    font-size: 12px;
                                    text-align: center;
                                    color: #777;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='container'>
                                <h2>Medical Appointment Cancellation</h2>
                                <p>Dear patient,</p>
                                <p>We regret to inform you that your medical appointment with Dr. {selectedDoctor.Name}, scheduled for <strong>{appointment.AppointmentDate:dddd, dd MMMM yyyy HH:mm}</strong>, has been cancelled.</p>
                                <p>If you wish to reschedule, please contact us.</p>
                                <p>Thank you for your understanding.</p>
                                <p class='footer'>This is an automated message. Do not reply to this email.</p>
                            </div>
                        </body>
                    </html>";

            // Create an email log object
            var emailLog = new EmailLog(appointment.Id, patientEmail);

            try
            {
                // Create the payload for the POST request
                var payload = new
                {
                    patientEmail = patientEmail,
                    subject = subject,
                    body = emailBody,
                    isHtml = true // Indicate that the content is HTML
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send the email using the Apps Script URL
                var response = await _httpClient.PostAsync(AppsScriptUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                // Log the response for debugging
                Console.WriteLine($"Response Status Code: {response.StatusCode}");
                Console.WriteLine($"Response Content: {responseContent}");

                // Set the email status based on the response
                if (response.IsSuccessStatusCode)
                {
                    emailLog.Status = EmailStatus.Sent;
                }
                else
                {
                    emailLog.Status = EmailStatus.NotSent;
                }
            }
            catch (Exception ex)
            {
                // Log any exception that occurs
                Console.WriteLine($"Exception: {ex.Message}");
                emailLog.Status = EmailStatus.NotSent;
            }
            finally
            {
                // Save the email log to the database
                emailLog.SentDate = DateTime.Now;
                await _emailLogRepo.CreateAsync(emailLog);
            }
        }

        /// <summary>
        /// Views the history of all sent emails.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ViewAllEmailHistoryAsync()
        {
            var emailLogs = await _emailLogRepo.GetAllAsync();

            if (emailLogs == null || emailLogs.Count == 0)
            {
                ConsoleUIHelpers.Warning("No emails found in the system.");
                ConsoleUIHelpers.Pause();
                return;
            }

            ConsoleUIHelpers.PrintHeader("📧 All Email History");

            // Display the details of each email log
            foreach (var emailLog in emailLogs)
            {
                var email = emailLog.Value;  // Accessing the EmailLog object

                Console.WriteLine($"Patient Email  : {email.RecipientEmail}");
                Console.WriteLine($"Sent Date      : {email.SentDate:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Status         : {email.Status}");
                Console.WriteLine(new string('-', 40)); // Separator between emails
            }

            ConsoleUIHelpers.Pause();
        }
    }
}
