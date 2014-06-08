using System;
using System.Collections.Generic;
using System.Configuration;
using MvcHelpers.Tests.Fakes;
using Xunit;
using AppSettingsReader = MvcHelpers.Services.AppSettingsReader;

namespace MvcHelpers.Tests
{
	public class AppSettingsReaderTests
	{
		[Fact]
		public void Should_Read_Value_From_Settings_That_Exist()
		{
			dynamic appSettings = new AppSettingsReader();

			var value = Convert.ToBoolean(appSettings.SettingExists);

			Assert.True(value);
		}


		[Fact]
		public void Should_Throw_Error_When_Reading_Value_From_Settings_That_Dont_Exist()
		{
			dynamic appSettings = new AppSettingsReader();

			Assert.Throws<KeyNotFoundException>(delegate
				{
					var val = appSettings.SettingDoesntExist;
				});
		}

		[Fact]
		public void Should_Return_One_ConnectionString_Named_TestConnectionString()
		{
			dynamic appSettings = new AppSettingsReader();

			var connectionStrings = (appSettings.ConnectionStrings as ConnectionStringSettingsCollection);
			
			Assert.NotNull(connectionStrings);
			Assert.Equal(1, connectionStrings.Count);
			Assert.Equal("TestConnectionString", connectionStrings[0].Name);
			Assert.Equal("This isn't a real connection string.", connectionStrings[0].ConnectionString);
		}

		[Fact]
		public void Should_Return_Two_FakeElements()
		{
			dynamic appSettings = new AppSettingsReader();

			var config = (FakeConfiguration)appSettings.GetSection("FakeConfiguration");

			Assert.Equal(2, config.FakeElementCollection.Count);
		}
	}
}
