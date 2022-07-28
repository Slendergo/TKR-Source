using Anna.Request;
using System;
using System.Collections.Specialized;
using NLog;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace server.account
{
    internal class passwordRecovery : RequestHandler
    {
        public static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public override void HandleRequest(RequestContext context, NameValueCollection query)
        {
            var acc = _db.GetAccount(query["guid"]);
            var timeNow = DateTime.UtcNow;

            if (acc != null && acc.NameChosen)
            {
                if(acc.LastRecoveryTime == null || (timeNow - acc.LastRecoveryTime).TotalDays > 1) // Passed more than one day?
                {
                    var token = GenerateRandomCryptographicKey(10);

                    acc.LastRecoveryTime = timeNow;
                    acc.FlushAsync();

                    try
                    {
                        if (!SendEmailTo(acc.UUID, acc.Name, token))
                        {
                            Write(context, "<Error>Something wrong happened, try again.</Error>");
                        }

                        _db.ChangePassword(acc.UUID, token);
                        Write(context, "Email sent with your new password!");
                    }
                    catch(Exception e)
                    {
                        Write(context, "<Error>Something wrong happened, try again later.</Error>");
                        Log.Error(e);
                    }
                }
                else
                {
                    acc.FlushAsync();
                    Write(context, "<Error>You need to wait at least 24hrs to use this feature again!</Error>");
                }
            }
            else
                Write(context, "<Error>" + "Account doesn't exist or the email it's wrong!" + "</Error>");
        }

        public string GenerateRandomCryptographicKey(int keyLength) // got it from here -> https://dotnetcodr.com/2016/10/05/generate-truly-random-cryptographic-keys-using-a-random-number-generator-in-net/
        {
            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public bool SendEmailTo(string email, string name, string password)
        {

            var to = new MailAddress(email);
            var from = new MailAddress("no-reply@tkgames.com");

            var credentials = new NetworkCredential("talismanskingdomgames@gmail.com", "Talismanskingdom!");

            var message = new MailMessage();
            message.To.Add(to);
            message.From = from;
            message.Subject = "TK Games: Password Recovery for Talisman's Kingdom";
            message.Body = $"Hello {name}! Here's your new password for your account: {password}" +
                $"\n\nRemember that You can only request a new password after 24 hours." +
                $"\n\nAlso, remember that you can change your password after log in it to your account. Check the button called \"Account\" in the Main Menu." +
                $"\n\nFor more details, contact moderators in our Discord: https://discord.gg/J6w6V6b" +
                $"\n\nKind regards, Staff." +
                $"\nTK Games";

            var client = new SmtpClient
            {
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Credentials = credentials
            };

            try
            {
                client.Send(message);
                return true;
            }
            catch(Exception e)
            {
                Log.Error(e);
                return false;
            }
            
        }
    }
}
