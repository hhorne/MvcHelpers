using System.Net.Mail;
using System.Threading.Tasks;
using MvcHelpers.Services;

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
			var emailBody = BuildEmailBody(instructions);
			var email = new HtmlEmail(instructions, emailBody);
			return email;
		}

		private string BuildEmailBody(DeliveryInstructions instructions)
		{
			var brand = instructions.Brand;
			var messageName = instructions.MessageTemplate;

			// Pull up message template and apply the data
			var messagePath = ioService.CombineAndMap(EmailPath, BrandedMessageFolder, brand, messageName);
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