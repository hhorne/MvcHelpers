using System;
using MvcHelpers.Services;
using Xunit;

namespace MvcHelpers.Tests.ServicesTests
{
	public class Sha256CryptoServiceTests
	{
		string password = "Correct Password 123";
		string badPassword = "Bad Password :(";
        Sha256CryptoService _CryptoService;

        public Sha256CryptoServiceTests()
        {
            _CryptoService = new Sha256CryptoService();
        }

		[Fact]
		public void Should_Make_Hashed_Password_128_Characters_Or_Greater()
		{
			string hashedPassword = _CryptoService.HashPassword(password);
			
			Assert.True(hashedPassword.Length >= 128);
		}

		[Fact]
		public void Should_Validate_Correct_Password()
		{
			string hashedPassword = _CryptoService.HashPassword(password);
			
			bool validPassword = _CryptoService.ValidatePassword(password, hashedPassword);
			
			Assert.True(validPassword);
		}

		[Fact]
		public void Should_Reject_Invalid_Password()
		{
			string hashedPassword = _CryptoService.HashPassword(password);
			
			bool validPassword = _CryptoService.ValidatePassword(badPassword, hashedPassword);
			
			Assert.False(validPassword);
		}

		[Fact]
		public void Should_Throw_Exception_When_correctHash_Is_Too_Short()
		{			
			string shortHash = _CryptoService.HashPassword(password);
			shortHash = shortHash.Substring(0, shortHash.Length - 1);
			
			Assert.Throws<ArgumentException>(()=> _CryptoService.ValidatePassword(password, shortHash));
		}
	}
}
