using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace MvcHelpers.Email
{
	public class DeliveryInstructions
	{
		public string Brand { get; set; }
		public List<string> RecipientList { get; set; }
		public MailAddress From { get; set; }
		public string Subject { get; set; }
		public string MessageTemplate { get; set; }
		public bool Enabled { get; set; }
		public Encoding BodyEncoding { get; set; }
		public dynamic Parameters { get; set; }

		public DeliveryInstructions()
		{
			BodyEncoding = Encoding.UTF8;
		}

		public string GetDelimitedRecipients(string delimiter)
		{
			var recipients = string.Join(delimiter, RecipientList);
			return recipients;
		}
	}
}
