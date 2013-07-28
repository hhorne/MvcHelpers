using System.IO;
using System.Web.Hosting;

namespace MvcHelpers.Services
{
	public interface IIOService
	{
		string MapPath(string path);
		string ReadAllText(string path);
		string Combine(string path1, string path2);
		string Combine(params string[] paths);
		string CombineAndMap(params string[] paths);
	}

	public class IOService : IIOService
	{
		public string MapPath(string path)
		{
			string mappedPath = HostingEnvironment.MapPath(path);
			return mappedPath;
		}

		public string ReadAllText(string path)
		{
			var result = File.ReadAllText(path);
			return result;
		}

		public string Combine(string path1, string path2)
		{
			var result = Path.Combine(path1, path2);
			return result;
		}

		public string Combine(params string[] paths)
		{
			var result = Path.Combine(paths);
			return result;
		}

		public string CombineAndMap(params string[] paths)
		{
			var result = Path.Combine(paths);
			result = HostingEnvironment.MapPath(result);

			return result;
		}
	}
}
