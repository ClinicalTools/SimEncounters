using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class User
    {
        public static User Guest { get; } = new User();

        public bool IsGuest { get; }
        public int AccountId { get; }
        public string Email { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Honorific { get; set; }

        // Creates guest user
        protected User()
        {
            IsGuest = true;
        }

        public User(int accountId, string email)
        {
            AccountId = accountId;
            Email = email;
        }
    }
}