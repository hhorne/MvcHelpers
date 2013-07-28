using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;

namespace MvcHelpers.Services
{
	public interface IAppSettingsService
	{
		string this[string settingName] { get; set; }
	}

	public class AppSettingsService : DynamicObject, IAppSettingsService
	{
		public string this[string settingName]
		{
			get
			{
				return ConfigurationManager.AppSettings[settingName];
			}
			set
			{
				ConfigurationManager.AppSettings[settingName] = value;
			}
		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			var key = binder.Name;
			if (ConfigurationManager.AppSettings.AllKeys.Contains(key))
			{
				result = ConfigurationManager.AppSettings[key];
				return true;
			}
			throw new KeyNotFoundException(string.Format("Key \"{0}\" was not found in the given dictionary", key));
		}

		public override IEnumerable<string> GetDynamicMemberNames()
		{
			return ConfigurationManager.AppSettings.AllKeys;
		}
	}
}