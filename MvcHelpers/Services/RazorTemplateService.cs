using RazorEngine;

namespace MvcHelpers.Services
{
	public interface IRazorTemplateResolver
	{
		string ResolveTemplate(string template, dynamic data);
	}

	public class RazorTemplateResolver : IRazorTemplateResolver
	{
		public string ResolveTemplate(string template, dynamic data)
		{
			var result = Razor.Parse(template, data);
			return result;
		}
	}
}
