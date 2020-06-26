﻿namespace ClinicalTools.SimEncounters
{
    public class User
    {
        public static User Guest { get; } = new User();

        public bool IsGuest { get; }
        public int AccountId { get; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Honorific { get; set; }
        public UserStatus Status { get; set; } = new UserStatus();

        // Creates guest user
        protected User()
        {
            IsGuest = true;

            // temporarily use CTI information as the guest user to allow local cases to be viewable
            AccountId = 25;
            Username = "cti";
        }

        public User(int accountId) => AccountId = accountId;


        public string GetName()
        {
            var name = FirstName + " " + LastName;
            if (!Honorific.Equals("--") && !string.IsNullOrWhiteSpace(Honorific))
                name = Honorific + " " + name;

            return name;
        }
    }
}