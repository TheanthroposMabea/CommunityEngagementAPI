using CommunityEngagementAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using System.Text;
using static System.Net.WebRequestMethods;

namespace StudentApp.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void sendEmailSettings(string toEmail, string subject, TextPart body)
        {
            var from = _config["EmailSettings:From"];
            var password = _config["EmailSettings:Password"];
            var host = _config["EmailSettings:Host"];
            var port = int.Parse(_config["EmailSettings:Port"]);

            if (string.IsNullOrWhiteSpace(from))
                throw new Exception("EmailSettings:From is missing");

            if (string.IsNullOrWhiteSpace(password))
                throw new Exception("EmailSettings:Password is missing");

            if (string.IsNullOrWhiteSpace(host))
                throw new Exception("EmailSettings:Host is missing");
            if (string.IsNullOrWhiteSpace(_config["EmailSettings:Port"]))
                throw new Exception("EmailSettings:Port is missing or invalid");


            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(from));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = body;

                var smtp = new SmtpClient();
                Console.WriteLine("EMAIL: Connecting..." + from + password + host + port);
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                Console.WriteLine("EMAIL: Connected!");
                Console.WriteLine("EMAIL: Authenticating...");
                smtp.Authenticate(from, password);
                Console.WriteLine("EMAIL: Authenticated!");
                Console.WriteLine("EMAIL: Sending...");
                smtp.Send(email);
                Console.WriteLine("EMAIL: Sent!");
                smtp.Disconnect(true);
                Console.WriteLine("EMAIL: Disconnected!");
            }
            catch (Exception) { }
        }
        public  async Task SendOTP(string toEmail, string otp)
        {
            //this is my information from appsettings.json for google to send the email
            //var from = _config["EmailSettings:From"];
            //var password = _config["EmailSettings:Password"];
            //var host = _config["EmailSettings:Host"];
            //var port = int.Parse(_config["EmailSettings:Port"]);


            //build the email
            //var email = new MimeMessage();
            //email.From.Add(MailboxAddress.Parse(sendSettings());
            //email.To.Add(MailboxAddress.Parse(toEmail));
           string   subject = "OTP Verification";
           TextPart body= new TextPart("html")
            {
               Text = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                        <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                            <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                        </div>

                        <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                           Thank you for registering! Please use the OTP below to verify your email address and continue with your registration.
                        </p>

                        <div style='text-align: center; margin: 24px 0;'>
                            <a href=''
                               style='background-color: #FF6B00; color: white; text-decoration: none; padding: 12px 28px; border-radius: 6px; font-size: 20px; font-weight: bold; display: inline-block;'>
                                {otp}
                            </a>
                        </div>

                        <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                            This OTP expires in 5 minutes. If you did not request an OTP, you can safely ignore this email.
                        </p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                        <p style='color: #999; font-size: 11px; text-align: center;'>
                            University of Johannesburg — Community Engagement<br/>
                            You're receiving this because you're registered on UJ CE.
                        </p>
                    </div>"
           };

            //send the email to the user
            sendEmailSettings(toEmail, subject, body);
         
          //Did some optimisation and it worked so had to comment these out

            //var smtp = new SmtpClient();
            //smtp.Connect(host, port);
            //smtp.Authenticate(from, password);
            //smtp.Send(email);
            //smtp.Disconnect(true);

        }

        public async Task sendLink(string email, string token)
        {
            
          //  token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string subject = "Reset Email Verification";
            TextPart body = new TextPart("html")
            {
                Text = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                        <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                            <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                        </div>

                        <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                            Hi, use the link below to reset your password. This link can only be used once.
                        </p>

                        <div style='text-align: center; margin: 24px 0;'>
                            <a href='http://localhost:5173/forgot-password?token={token}&email={Uri.EscapeDataString(email)}'
                               style='background-color: #FF6B00; color: white; text-decoration: none; padding: 12px 28px; border-radius: 6px; font-size: 14px; font-weight: bold; display: inline-block;'>
                                Reset Password
                            </a>
                        </div>

                        <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                            This link expires in 5 minutes. If you did not request a password reset, you can safely ignore this email.
                        </p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                        <p style='color: #999; font-size: 11px; text-align: center;'>
                            University of Johannesburg — Community Engagement<br/>
                            You're receiving this because you're registered on UJ CE.
                        </p>
                    </div>"
                // Text = $"http://localhost:5173/forgot-password?token={token}&email={Uri.EscapeDataString(email)}"
            };

            sendEmailSettings(email, subject, body);  
        }

        //for unregistered users
          public async Task sendRegistrationLink(string email)
        {
            
          //  token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string subject = "UJ Community Engagement Registration";
            TextPart body = new TextPart("html")
            {
                Text = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                        <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                            <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                        </div>

                        <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                            Hi, you have recently volunteer together with us, WHy not join the family?Kindly receive the link to go to our registration page.
                        </p>

                        <div style='text-align: center; margin: 24px 0;'>
                            <a href='http://localhost:5173/register'
                               style='background-color: #FF6B00; color: white; text-decoration: none; padding: 12px 28px; border-radius: 6px; font-size: 14px; font-weight: bold; display: inline-block;'>
                                Register Link
                            </a>
                        </div>

                        <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                            Thank You. Hoping to make more impact with you.
                        </p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                        <p style='color: #999; font-size: 11px; text-align: center;'>
                            University of Johannesburg — Community Engagement<br/>
                            You're receiving this because you're registered on UJ CE.
                        </p>
                    </div>"
                // Text = $"http://localhost:5173/forgot-password?token={token}&email={Uri.EscapeDataString(email)}"
            };

            sendEmailSettings(email, subject, body);  
        }






        public async Task sendReuploadLink(string email, string token, int application_id)
        {

            //  token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            string subject = "Application Documents Correction";
            TextPart body = new TextPart("html")
            {
                Text = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                        <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                            <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                        </div>

                        <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                            Hi, please reupload your application documents using the link below. Make sure the correct files are attached before submitting.
                        </p>

                        <div style='text-align: center; margin: 24px 0;'>
                            <a href='http://localhost:5173/ngo/reupload-documents?token={token}&email={Uri.EscapeDataString(email)}&application_id={application_id}'
                               style='background-color: #FF6B00; color: white; text-decoration: none; padding: 12px 28px; border-radius: 6px; font-size: 14px; font-weight: bold; display: inline-block;'>
                                Reupload Documents
                            </a>
                        </div>

                        <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                            This link is valid for 7 days. If you did not request this, no action is needed.
                        </p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                        <p style='color: #999; font-size: 11px; text-align: center;'>
                            University of Johannesburg — Community Engagement<br/>
                            You're receiving this because you're registered on UJ CE.
                        </p>
                    </div>"
                // Text = $"http://localhost:5173/forgot-password?token={token}&email={Uri.EscapeDataString(email)}"
            };
            sendEmailSettings(email, subject, body);
        }
        public async Task statusUpdateEmail(string email, Status status)
        {
            string subject = "UJ CBO Partnership Status Update";
            TextPart body = new TextPart("html")
            {
                Text = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                        <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                            <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                        </div>

                        <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                            Hi, your status on UJ CE has changed.
                        </p>

                        <table style='width: 100%; margin: 20px 0;'>
                            <tr>
                                <td style='background-color: #FFF3EA; border-left: 4px solid #FF6B00; padding: 14px 18px; border-radius: 4px;'>
                                    <span style='color: #888; font-size: 12px; text-transform: uppercase;'>New status</span><br/>
                                    <span style='color: #FF6B00; font-size: 20px; font-weight: bold;'>{status}</span>
                                </td>
                            </tr>
                        </table>

                        <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                            You can review this change by logging into your account.
                        </p>

                        <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                        <p style='color: #999; font-size: 11px; text-align: center;'>
                            University of Johannesburg — Community Engagement<br/>
                            You're receiving this because you're registered on UJ CE.
                        </p>
                    </div>"
            };

            //send the email to the user
            sendEmailSettings(email, subject, body);

        }
        public async Task ActivationUpdate(string email, string activation)
        {
            string subject = "Account Activation Update";
            TextPart body = new TextPart("html")
            {
                // 1. Activation status changed
                Text = $@"
                <div style='font-family: Arial, sans-serif; max-width: 500px; margin: 0 auto; padding: 24px; border: 1px solid #eee; border-radius: 8px;'>
                    <div style='border-bottom: 3px solid #FF6B00; padding-bottom: 12px; margin-bottom: 20px;'>
                        <h2 style='color: #222; margin: 0;'>UJ Community Engagement</h2>
                    </div>

                    <p style='color: #444; font-size: 15px; line-height: 1.5;'>
                        Hi, your account activation status has been updated by your specialist.
                    </p>

                    <table style='width: 100%; margin: 20px 0;'>
                        <tr>
                            <td style='background-color: #FFF3EA; border-left: 4px solid #FF6B00; padding: 14px 18px; border-radius: 4px;'>
                                <span style='color: #888; font-size: 12px; text-transform: uppercase;'>Status</span><br/>
                                <span style='color: #FF6B00; font-size: 20px; font-weight: bold;'>{activation}</span>
                            </td>
                        </tr>
                    </table>

                    <p style='color: #666; font-size: 13px; line-height: 1.5;'>
                        If you have questions about this change, please contact your assigned specialist.
                    </p>

                    <hr style='border: none; border-top: 1px solid #eee; margin: 24px 0;'/>

                    <p style='color: #999; font-size: 11px; text-align: center;'>
                        University of Johannesburg — Community Engagement<br/>
                        You're receiving this because you're registered on UJ CE.
                    </p>
                  </div>"
            };

            //send the email to the user
            sendEmailSettings(email, subject, body);

        }
        //create a welcome email for the user
        public async Task WelcomeEmail(
    
    string fullName,
    string userEmail,
    string campusName,
    string specialistEmail)
        {
            string subject = "Welcome to the UJ Community Engagement Volunteer Programme";

            TextPart body = new TextPart("html")
            {
                Text = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='UTF-8'>
                    </head>
                    <body style='margin:0; padding:0; background-color:#f5f5f5; font-family:Arial, Helvetica, sans-serif;'>

                    <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f5f5f5; padding:30px 0;'>
                    <tr>
                    <td align='center'>

                    <table width='650' cellpadding='0' cellspacing='0' style='background:#ffffff; border:1px solid #dddddd;'>

                    <tr>
                    <td style='padding:40px; color:#333333; font-size:16px; line-height:1.7;'>

                    <p style='margin-top:0;'>Dear <strong>{fullName}</strong>,</p>

                    <p>
                        <span style='background-color:#d9ecff;
                                     color:#1e73be;
                                     font-weight:bold;
                                     padding:2px 6px;
                                     border-radius:2px;'>
                            Congratulations!
                        </span>
                        Your registration for the
                        <strong>University of Johannesburg Community Engagement Volunteer Programme</strong>
                        has been successfully completed.
                    </p>

                    <p>
                        We are excited to have you join us on this journey of making a positive
                        impact within our community.
                    </p>

                    <h3 style='color:#f58220; margin-top:35px; margin-bottom:10px;'>
                        Next Steps:
                    </h3>

                    <p>
                        For accessibility and convenience, we recommend using your mobile device.
                    </p>

                    <p>
                        Please log in using your student email
                        <strong>{userEmail}</strong>
                        and the password you created
                        <strong>(not your ULink password)</strong>
                        to access the app.
                        Ensure that you turn on notifications to stay updated on important
                        announcements and volunteer opportunities.
                    </p>

                    <h3 style='color:#f58220; margin-top:35px; margin-bottom:10px;'>
                        Campus-Specific Details:
                    </h3>
                    <p>
                        <strong>{campusName}</strong><br>
                        For any inquiries or support related to the programme,
                        please visit the <strong>E-Les G15 Office</strong>.
                    </p>
                    <p>
                        You can also contact your Volunteer Champion Ambassador at
                        <a href='mailto:{specialistEmail}'
                           style='color:#1e73be; text-decoration:none;'>
                           {specialistEmail}
                        </a>.
                    </p>

                    <p>
                        If you have any questions or require further assistance,
                        you can also contact us at
                        <a href='mailto:info-ce@uj.ac.za'
                           style='color:#1e73be; text-decoration:none;'>
                           info-ce@uj.ac.za
                        </a>.
                    </p>

                    <h3 style='color:#f58220; margin-top:35px; margin-bottom:10px;'>
                        Stay Connected!
                    </h3>

                    <p>
                        Follow our pages to stay engaged and get the latest updates on volunteer
                        opportunities and events:
                    </p>

                    <p style='margin-bottom:5px;'>
                        <strong>Instagram:</strong>
                        <span style='color:#1e73be;'>@uj_communityengagement</span>
                    </p>

                    <p style='margin-top:0;'>
                        <strong>Facebook:</strong>
                        <span style='color:#1e73be;'>UJ Community Engagement</span>
                    </p>

                    <hr style='border:none; border-top:1px solid #e5e5e5; margin:35px 0;'>

                    <p>
                        Thank you for your commitment to making a difference!
                        Together, we will create impactful change.
                    </p>

                    <p style='margin-top:35px;'>
                        Best regards,<br>
                        <strong>University of Johannesburg Community Engagement</strong>
                    </p>

                    </td>
                    </tr>

                    </table>

                    </td>
                    </tr>
                    </table>

                    </body>
                    </html>"
            };

            // Send email
            sendEmailSettings(userEmail, subject, body);
        }
    }
}
