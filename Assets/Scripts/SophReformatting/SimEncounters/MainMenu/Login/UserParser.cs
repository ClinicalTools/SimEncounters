using System;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class UserParser
    {
        private const int USER_PARTS = 6;
        private const int ACCOUNT_ID_INDEX = 2;
        private const int USERNAME_INDEX = 3;
        private const int EMAIL_INDEX = 4;
        private const int FIRST_NAME_INDEX = 6;
        private const int LAST_NAME_INDEX = 7;
        public User Parse(string userText)
        {
            var splitChars = new string[] { "--" };
            var userParts = userText.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            if (userParts.Length < USER_PARTS)
                return null;

            if (!int.TryParse(userParts[ACCOUNT_ID_INDEX], out int accountId))
                return null;

            return new User(accountId) {
                Username = userParts[USERNAME_INDEX],
                Email = userParts[EMAIL_INDEX]
            };
        }

    }
}