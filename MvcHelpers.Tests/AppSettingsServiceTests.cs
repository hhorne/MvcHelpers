using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcHelpers.Services;
using Xunit;

namespace MvcHelpers.Tests
{
	public class AppSettingsServiceTests
	{
		[Fact]
		public void Should_Read_Value_From_Settings_That_Exist()
		{
			dynamic appSettings = new AppSettingsService();

			var value = appSettings.SettingExists;

			Assert.Equal("true", value);
		}


		[Fact]
		public void Should_Throw_Error_When_Reading_Value_From_Settings_That_Dont_Exist()
		{
			dynamic appSettings = new AppSettingsService();

			var value = appSettings.SettingDoesntExist;

			Assert.Equal("true", value);
		}
	}
}
