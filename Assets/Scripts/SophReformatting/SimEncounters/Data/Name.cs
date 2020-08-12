using System;

namespace ClinicalTools.SimEncounters.Data
{
    public class Name : IComparable<Name>
    {
        public virtual string Honorific { get; set; } = "";
        public virtual string FirstName { get; set; } = "";
        public virtual string LastName { get; set; } = "";

        public Name() { }
        public Name(string name)
        {
            var nameParts = name.Split(' ');
            if (nameParts.Length == 0)
                return;
            LastName = nameParts[nameParts.Length - 1];
            if (nameParts.Length == 1)
                return;
            FirstName = nameParts[nameParts.Length - 2];
            if (nameParts.Length == 3)
                Honorific = nameParts[0];
        }

        public Name(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public Name(string title, string firstName, string lastName)
        {
            Honorific = title;
            FirstName = firstName;
            LastName = lastName;
        }


        public override string ToString() => string.Join(" ", Honorific, FirstName, LastName);

        public int CompareTo(Name other)
        {
            int value = LastName.CompareTo(other.LastName);
            if (value != 0)
                return value;
            value = FirstName.CompareTo(other.FirstName);
            if (value != 0)
                return value;
            return Honorific.CompareTo(other.Honorific);
        }
    }
}