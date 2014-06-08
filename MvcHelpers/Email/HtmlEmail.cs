using System.Net.Mail;

namespace MvcHelpers.Email
{
	public class HtmlEmail : MailMessage
	{
		public HtmlEmail()
			: base()
		{
			base.IsBodyHtml = true;
		}

		public HtmlEmail(DeliveryInstructions instructions, string body)
			: base(instructions.From.Address, instructions.GetDelimitedRecipients(","), instructions.Subject, body)
		{
			base.BodyEncoding = instructions.BodyEncoding;
			base.IsBodyHtml = true;
		}
	}
}