using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MvcHelpers.Services;
using ServiceStack.Logging;

namespace MvcHelpers.Email
{
	public interface IMailer
	{
		Task SendEmail(MailMessage message);
	}

	public class Mailer : IMailer
	{
		private readonly dynamic appSettings;
		
		public Mailer(IAppSettingsReader appSettings)
		{
			this.appSettings = appSettings;
		}

		public async Task SendEmail(MailMessage message)
		{
			using (var mailClient = CreateMailClient())
			{
				await mailClient.SendMailAsync(message);
			}
		}

		private NetworkCredential CreateMailCredential()
		{
			string mailUsername = appSettings.MailUsername;
			string mailPassword = appSettings.MailPassword;
			var credential = new NetworkCredential(mailUsername, mailPassword);

			return credential;
		}

		private SmtpClient CreateMailClient()
		{
			string mailHost = appSettings.MailHost;
			int mailPort = int.Parse(appSettings.MailPort);

			var mailClient = new SmtpClient(mailHost, mailPort)
			{
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = CreateMailCredential(),
			};

			return mailClient;
		}
	}
}