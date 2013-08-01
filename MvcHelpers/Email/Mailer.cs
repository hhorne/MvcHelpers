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
		private readonly ILog log;

		public Mailer(IAppSettingsReader appSettings)
		{
			this.log = LogManager.GetLogger(GetType());
			this.appSettings = appSettings;
		}

		public async Task SendEmail(MailMessage message)
		{
			try
			{
				using (var mailClient = CreateMailClient())
				{
					await mailClient.SendMailAsync(message);
				}
			}
			catch (Exception e)
			{
				log.Error("Exception in Mailer.SendEmail", e);
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