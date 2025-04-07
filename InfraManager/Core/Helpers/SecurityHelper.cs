using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InfraManager.Core.Helpers
{
    public static class SecurityHelper
	{
		private const string __passwordCharsLowerCase = "abcdefgijkmnopqrstwxyz";
		private const string __passwordCharsUpperCase = "ABCDEFGHJKLMNPQRSTWXYZ";
		private const string __passwordCharsNumeric = "1234567890";
		private const string __passwordCharsSpecial = "*$-+?_&=!%{}/";
		private const int __defaultMinPasswordLength = 6;
		private const int __defaultMaxPasswordLength = 8;

        private const int LENGTH_OF_KEY = 16;
        const string __sw = "kjviq_371$#xna09mcaytlywbq";


        //#region method SetupGenericPrincipal
        //public static GenericPrincipal SetupGenericPrincipal(string identity, AuthenticationType authenticationType)
        //{
        //    var genericIdentity = new GenericIdentity(identity, Enum.GetName(typeof(AuthenticationType), authenticationType));
        //    var genericPrincipal = new GenericPrincipal(genericIdentity, null);
        //    Thread.CurrentPrincipal = genericPrincipal;
        //    return genericPrincipal;
        //}
        //#endregion

        #region method GeneratePassword
        public static string GeneratePassword()
		{
			return GeneratePassword(__defaultMinPasswordLength, __defaultMaxPasswordLength);
		}

		public static string GeneratePassword(int length)
		{
			return GeneratePassword(length, length);
		}

		public static string GeneratePassword(int minLength, int maxLength)
		{
			if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
				return null;
			//
			// Create a local array containing supported password characters
			// grouped by types. You can remove character groups from this
			// array, but doing so will weaken the password strength.
			char[][] charGroups = new char[][] 
                {
                    __passwordCharsLowerCase.ToCharArray(),
                    __passwordCharsUpperCase.ToCharArray(),
                    __passwordCharsNumeric.ToCharArray(),
                    __passwordCharsSpecial.ToCharArray()
                };
			//
			// Use this array to track the number of unused characters in each
			// character group.
			int[] charsLeftInGroup = new int[charGroups.Length];
			//
			// Initially, all characters in each group are not used.
			for (int i = 0; i < charsLeftInGroup.Length; i++)
				charsLeftInGroup[i] = charGroups[i].Length;
			//
			// Use this array to track (iterate through) unused character groups.
			int[] leftGroupsOrder = new int[charGroups.Length];
			//
			// Initially, all character groups are not used.
			for (int i = 0; i < leftGroupsOrder.Length; i++)
				leftGroupsOrder[i] = i;
			//
			// Because we cannot use the default randomizer, which is based on the
			// current time (it will produce the same "random" number within a
			// second), we will use a random number generator to seed the
			// randomizer.
			//
			// Use a 4-byte array to fill it with random bytes and convert it then
			// to an integer value.
			byte[] randomBytes = new byte[4];
			//
			// Generate 4 random bytes.
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);
			//
			// Convert 4 bytes into a 32-bit integer value.
			int seed = (randomBytes[0] & 0x7f) << 24 |
						randomBytes[1] << 16 |
						randomBytes[2] << 8 |
						randomBytes[3];
			//
			// Now, this is real randomization.
			Random random = new Random(seed);
			//
			// This array will hold password characters.
			char[] password = null;
			//
			// Allocate appropriate memory for the password.
			if (minLength < maxLength)
				password = new char[random.Next(minLength, maxLength + 1)];
			else
				password = new char[minLength];
			//
			// Index of the next character to be added to password.
			int nextCharIdx;
			// Index of the next character group to be processed.
			int nextGroupIdx;
			// Index which will be used to track not processed character groups.
			int nextLeftGroupsOrderIdx;
			// Index of the last non-processed character in a group.
			int lastCharIdx;
			// Index of the last non-processed group.
			int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
			//
			// Generate password characters one at a time.
			for (int i = 0; i < password.Length; i++)
			{
				// If only one character group remained unprocessed, process it;
				// otherwise, pick a random character group from the unprocessed
				// group list. To allow a special character to appear in the
				// first position, increment the second parameter of the Next
				// function call by one, i.e. lastLeftGroupsOrderIdx + 1.
				if (lastLeftGroupsOrderIdx == 0)
					nextLeftGroupsOrderIdx = 0;
				else
					nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx);
				//
				// Get the actual index of the character group, from which we will
				// pick the next character.
				nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];
				//
				// Get the index of the last unprocessed characters in this group.
				lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

				// If only one unprocessed character is left, pick it; otherwise,
				// get a random character from the unused character list.
				if (lastCharIdx == 0)
					nextCharIdx = 0;
				else
					nextCharIdx = random.Next(0, lastCharIdx + 1);
				//
				// Add this character to the password.
				password[i] = charGroups[nextGroupIdx][nextCharIdx];
				//
				// If we processed the last character in this group, start over.
				if (lastCharIdx == 0)
					charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
				// There are more unprocessed characters left.
				else
				{
					// Swap processed character with the last unprocessed character
					// so that we don't pick it until we process all characters in
					// this group.
					if (lastCharIdx != nextCharIdx)
					{
						char temp = charGroups[nextGroupIdx][lastCharIdx];
						charGroups[nextGroupIdx][lastCharIdx] =
									charGroups[nextGroupIdx][nextCharIdx];
						charGroups[nextGroupIdx][nextCharIdx] = temp;
					}
					// Decrement the number of unprocessed characters in
					// this group.
					charsLeftInGroup[nextGroupIdx]--;
				}
				//
				// If we processed the last group, start all over.
				if (lastLeftGroupsOrderIdx == 0)
					lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
				// There are more unprocessed groups left.
				else
				{
					// Swap processed group with the last unprocessed group
					// so that we don't pick it until we process all groups.
					if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
					{
						int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
						leftGroupsOrder[lastLeftGroupsOrderIdx] =
									leftGroupsOrder[nextLeftGroupsOrderIdx];
						leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
					}
					// Decrement the number of unprocessed groups.
					lastLeftGroupsOrderIdx--;
				}
			}
			//
			return new string(password);
		}
        #endregion


        #region static method Encrypt
        public static string Encrypt(string message)
        {
            if (message == null)
                return null;
            return Encrypt(message, __sw);
        }
        public static string Encrypt(string message, string keyCrypt)
        {
            var bMessage = Encoding.UTF8.GetBytes(message);
            var encriptedData = Encrypt(bMessage, keyCrypt);
            return Convert.ToBase64String(encriptedData);
        }
        private static byte[] Encrypt(byte[] message, string keyCrypt)
        {
            ICryptoTransform transform = null;
            byte[] retval = null;
            try
            {
                using (SymmetricAlgorithm sa = Rijndael.Create())
                using (Rfc2898DeriveBytes db = new Rfc2898DeriveBytes(keyCrypt, new byte[LENGTH_OF_KEY]))
                {
                    var rgbKey = db.GetBytes(LENGTH_OF_KEY);
                    var rgbV = new byte[LENGTH_OF_KEY];
                    transform = sa.CreateEncryptor(rgbKey, rgbV);
                }
                //
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    cs.Write(message, 0, message.Length);
                    cs.FlushFinalBlock();
                    retval = ms.ToArray();
                    //
                    cs.Close();
                    ms.Close();
                }
            }
            finally
            {
                transform.Dispose();
            }
            return retval;
        }
        #endregion

        #region static method Decrypt
        public static string Decrypt(string message)
        {
            if (message == null)
                return null;
            return Decrypt(message, __sw);
        }
        public static string Decrypt(string message, string keyCrypt)
        {
            var bMessage = Convert.FromBase64String(message);
            var decriptedData = Decrypt(bMessage, keyCrypt);
            var retval = Encoding.UTF8.GetString(decriptedData);
            return retval;
        }
        private static byte[] Decrypt(byte[] message, string keyCrypt)
        {
            byte[] retval = null;
            ICryptoTransform transform = null;
            try
            {
                using (SymmetricAlgorithm sa = Rijndael.Create())
                using (Rfc2898DeriveBytes db = new Rfc2898DeriveBytes(keyCrypt, new byte[LENGTH_OF_KEY]))
                {
                    var rgbKey = db.GetBytes(LENGTH_OF_KEY);
                    var rgbV = new byte[LENGTH_OF_KEY];
                    transform = sa.CreateDecryptor(rgbKey, rgbV);
                }
                //
                using (MemoryStream ms = new MemoryStream(message))
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    var data = sr.ReadToEnd();
                    retval = Encoding.UTF8.GetBytes(data);
                    //
                    sr.Close();
                    cs.Close();
                    ms.Close();
                }
            }
            catch (CryptographicException)
            {
                retval = null;
            }
            return retval;
        }
        #endregion
    }
}
