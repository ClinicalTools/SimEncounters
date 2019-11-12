using System.Collections.Generic;
using System.Xml;

namespace SimEncounters
{
    /// <summary>
    /// Stores an ordered collection of unique values by randomly generated key strings.
    /// </summary>
    /// <typeparam name="T">
    /// Type of the values in the collection. Type cannot be a string or an int.
    /// </typeparam>
    public abstract class OrderedCollection<T> : SimCollection<T>
    {
        // An ordered list of sections
        protected virtual List<T> ValueList { get; set; } = new List<T>();
        /// <summary>
        /// Ordered array of values in the collection.
        /// </summary>
        public virtual T[] ValueArr => ValueList.ToArray();
        public override IEnumerable<T> Values => ValueList;

        public OrderedCollection() : base() { }
        public OrderedCollection(XmlNode encounterNode) : base(encounterNode) { }

        protected override void Add(string key, T value)
        {
            base.Add(key, value);
            ValueList.Add(value);
        }

        public override void Remove(string key)
        {
            var item = Collection[key];
            ValueList.Remove(item);
            base.Remove(key);
        }
        
        public virtual T Get(int val) => ValueList[val];

        public virtual void MoveValue(int newIndex, int currentIndex)
        {
            ValueList.Insert(newIndex, ValueList[currentIndex]);
            if (newIndex < currentIndex)
                currentIndex++;

            ValueList.RemoveAt(currentIndex);
        }
        public virtual void MoveValue(int newIndex, T value)
        {
            ValueList.Remove(value);
            ValueList.Insert(newIndex, value);
        }
    }
}
