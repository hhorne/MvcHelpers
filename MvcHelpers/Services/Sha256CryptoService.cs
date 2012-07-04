using System;
using System.Security.Cryptography;
using System.Text;

namespace MvcHelpers.Services
{
	public interface ICryptoService
	{
		string HashPassword(string password);
		bool ValidatePassword(string password, string correctHash);
	}

	public class Sha256CryptoService : ICryptoService
	{
		private readonly SHA256 _HashProvider;
		private readonly RNGCryptoServiceProvider _SaltGenerator;

		public Sha256CryptoService()
		{
			_HashProvider = new SHA256Managed();
			_SaltGenerator = new RNGCryptoServiceProvider();
		}
		
		/// <summary>
		/// Hashes a password
		/// </summary>
		/// <param name="password">The password to hash</param>
		/// <returns>The hashed password as a 128 character hex string</returns>
		public string HashPassword(string password)
		{
			string salt = GetRandomSalt();
			string hash = Sha256Hex(salt + password);
			return salt + hash;
		}

		/// <summary>
		/// Validates a password
		/// </summary>
		/// <param name="password">The password to test</param>
		/// <param name="correctHash">The hash of the correct password</param>
		/// <returns>True if password is the correct password, false otherwise</returns>
		public bool ValidatePassword(string password, string correctHash)
		{
			if (correctHash.Length < 128)
			{
				throw new ArgumentException("correctHash must be 128 hex characters!");
			}

			string salt = correctHash.Substring(0, 64);
			string validHash = correctHash.Substring(64, 64);
			string passHash = Sha256Hex(salt + password);
			return string.Compare(validHash, passHash) == 0;
		}

		private string Sha256Hex(string password)
		{
			byte[] utf8 = UTF8Encoding.UTF8.GetBytes(password);
			return BytesToHex(_HashProvider.ComputeHash(utf8));
		}

		//Returns a random 64 character hex string (256 bits)
		private string GetRandomSalt(int size = 32)
		{
			byte[] salt = new byte[size]; //256 bits
			_SaltGenerator.GetBytes(salt);
			return BytesToHex(salt);
		}

		//Converts a byte array to a hex string
		private string BytesToHex(byte[] toConvert)
		{
			var buffer = new StringBuilder(toConvert.Length * 2);
			foreach (byte rawByte in toConvert)
			{
				buffer.Append(rawByte.ToString("x2"));
			}
			return buffer.ToString();
		}
	}
}
