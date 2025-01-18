using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ContactManager.Models;
using SendGrid;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using ContactManager.tr.com.dataport.api;

namespace ContactManager
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configSendGridasync(message);
        }

        // Use NuGet to install SendGrid (Basic C# client lib) 
        private async Task configSendGridasync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(message.Destination);
            myMessage.From = new System.Net.Mail.MailAddress(
                                "Joe@contoso.com", "Joe S.");
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.Body;

            var credentials = new NetworkCredential(
                       ConfigurationManager.AppSettings["SendGridMailAccount"],
                       ConfigurationManager.AppSettings["SendGridMailPassword"]
                       );

            // Create a Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            if (transportWeb != null)
            {
                await transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                Trace.TraceError("Failed to create Web transport.");
                await Task.FromResult(0);
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            #region Twilio
            // Plug in your SMS service here to send a text message.

            //var Twilio = new TwilioRestClient(ConfigurationManager.AppSettings["TwilioSid"],
            //    ConfigurationManager.AppSettings["TwilioToken"]);

            //var result = Twilio.SendMessage(
            //    ConfigurationManager.AppSettings["TwilioFromPhone"],
            //   message.Destination, message.Body);

            //// Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
            //Trace.TraceInformation(result.Status);

            //// Twilio doesn't currently have an async API, so return success. 
            #endregion

            #region WebClient
            //WebClient client = new WebClient();
            //// Add a user agent header in case the requested URI contains a query.
            //client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");
            //client.QueryString.Add("user", ConfigurationManager.AppSettings["ClickatellUser"]);
            //client.QueryString.Add("password", ConfigurationManager.AppSettings["ClickatellPassword"]);
            //client.QueryString.Add("api_id", ConfigurationManager.AppSettings["ClickatellApi_id"]);
            //client.QueryString.Add("to", message.Destination);
            //client.QueryString.Add("text", message.Body);
            //string baseurl = ConfigurationManager.AppSettings["ClickatellBaseUrl"];
            //Stream data = client.OpenRead(baseurl);
            //StreamReader reader = new StreamReader(data);
            //string s = reader.ReadToEnd();
            //data.Close();
            //reader.Close();
            ////Trace.TraceInformation(s);
            //Trace.TraceInformation(message.Body);
            ////return (s);
            #endregion

            #region RestSharp
            //var client = new RestClient(ConfigurationManager.AppSettings["ClickatellBaseUrl"]);
            // client.Authenticator = new HttpBasicAuthenticator(username, password);

            //var request = new RestRequest("", Method.GET);
            //// adds to POST or URL querystring based on Method
            //request.AddParameter("user", ConfigurationManager.AppSettings["ClickatellUser"]);
            //request.AddParameter("password", ConfigurationManager.AppSettings["ClickatellPassword"]);
            //request.AddParameter("api_id", ConfigurationManager.AppSettings["ClickatellApi_id"]);
            //request.AddParameter("to", message.Destination);
            //request.AddParameter("text", message.Body);
            //request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

            // easily add HTTP Headers
            //request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2228.0 Safari/537.36");

            //// add files to upload (works with compatible verbs)
            //request.AddFile(path);

            //// execute the request
            //RestResponse response = client.Execute(request);
            //var content = response.Content; // raw content as string

            //// or automatically deserialize result
            //// return content type is sniffed but can be explicitly set via RestClient.AddHandler();
            //RestResponse<Person> response2 = client.Execute<Person>(request);
            //var name = response2.Data.Name;

            //// easy async support
            //RestRequestAsyncHandle task = client.ExecuteAsync(request, response =>
            //{
            //    Console.WriteLine(response.Content);
            //});

            ////// async with deserialization
            //var asyncHandle = client.ExecuteAsync<Person>(request, response =>
            //{
            //    Console.WriteLine(response.Data.Name);
            //});

            //// abort the request on demand
            //asyncHandle.Abort(); 
            #endregion

            #region DataPort Web Service .asmx

            MessageServices MS = new MessageServices();

            SendSMSRequest sms = new SendSMSRequest();

            Session ses = new Session();
            ses.AccountNumber = ConfigurationManager.AppSettings["DataPortAccountNumber"];
            ses.UserName = ConfigurationManager.AppSettings["DataPortUserName"];
            ses.Password = ConfigurationManager.AppSettings["DataPortPassword"];
            string session = MS.Register(ses);

            if (message.Subject == "Turkcell")
                sms.Operator = Operators.Turkcell;
            else if (message.Subject == "Avea")
                sms.Operator = Operators.Avea;
            else if (message.Subject == "Vodafone")
                sms.Operator = Operators.Vodafone;

            sms.SessionID = session;
            sms.ShortNumber = ConfigurationManager.AppSettings["DataPortServiceCode"];
            sms.Orginator = ConfigurationManager.AppSettings["DataPortAlphaNumeric"];
            sms.Isunicode = Unicode.No;
            sms.SendDate = DateTime.Now.ToString();
            sms.DeleteDate = ""; // Unknown
            sms.GroupID = "0";

            MessageList ML = new MessageList();
            ML.ContentList = new Content[] { new Content() { Value = message.Body } };
            ML.GSMList = new GSM[] { new GSM() { Value = message.Destination } };
            sms.MessageList = ML;


            SendMessageResponse rl = MS.SendSMS(sms);

            #endregion

            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
