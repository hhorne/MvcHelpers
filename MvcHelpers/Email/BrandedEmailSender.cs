using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MvcHelpers.Email;
using MvcHelpers.Services;
using log4net;

namespace MvcHelpers.Email
{
	public interface IBrandedEmailSender
	{
		string EmailPath { get; set; }
		string BrandedTemplateFolder { get; set; }
		string BrandedMessageFolder { get; set; }

		Task SendEmail(DeliveryInstructions instructions);
	}

	public class BrandedEmailSender : IBrandedEmailSender
	{
		private readonly IIOService ioService;
		private readonly IRazorTemplateResolver templateResolver;
		private readonly IMailer mailer;

		public string EmailPath { get; set; }
		public string BrandedTemplateFolder { get; set; }
		public string BrandedMessageFolder { get; set; }

		public BrandedEmailSender(IIOService ioService, IRazorTemplateResolver templateResolver, IMailer mailer)
		{
			this.ioService = ioService;
			this.templateResolver = templateResolver;
			this.mailer = mailer;

			EmailPath = "~/Emails";
			BrandedTemplateFolder = "BrandTemplates";
			BrandedMessageFolder = "Messages";
		}

		public async Task SendEmail(DeliveryInstructions instructions)
		{
			if (instructions.Enabled == false)
			{
				return;
			}

			var email = BuildEmailMessage(instructions);
			await mailer.SendEmail(email);
		}

		private MailMessage BuildEmailMessage(DeliveryInstructions instructions)
		{
			var recipients = instructions.GetDelimitedRecipients(", ");
			var email = new MailMessage();

			email.To.Add(recipients);
			email.From = instructions.From;
			email.Subject = instructions.Subject;
			email.BodyEncoding = instructions.BodyEncoding;
			email.IsBodyHtml = true;
			email.Body = BuildEmailBody(instructions);

			return email;
		}

		private string BuildEmailBody(DeliveryInstructions instructions)
		{
			var brand = instructions.Brand;
			var messageTemplateName = instructions.MessageTemplate;

			// Pull up message template and apply the data
			var messagePath = ioService.CombineAndMap(EmailPath, BrandedMessageFolder, brand, messageTemplateName);
			var messageTemplate = ioService.ReadAllText(messagePath);

			var message = templateResolver.ResolveTemplate(messageTemplate, instructions.Parameters);

			// Pull up brand template and apply the message
			var brandTemplatePath = ioService.CombineAndMap(EmailPath, BrandedTemplateFolder, brand + ".cshtml");
			var brandTemplate = ioService.ReadAllText(brandTemplatePath);

			var body = templateResolver.ResolveTemplate(brandTemplate, new { Body = message });
			return body;
		}
	}
}