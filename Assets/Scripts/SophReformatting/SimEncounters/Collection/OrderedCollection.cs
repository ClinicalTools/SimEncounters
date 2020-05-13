using System.Collections;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Collections
{
    /// <summary>
    /// An ordered bidirectional dictionary with automatically generated string keys.
    /// </summary>
    /// <typeparam name="T">Type of values stored in the collection</typeparam>
    public class OrderedCollection<T> : KeyedCollection<T>, IEnumerable<KeyValuePair<string, T>>
    {
        // An ordered list of values
        protected virtual List<T> ValueList { get; set; } = new List<T>();
        // An ordered list of the key value pairs
        // This could be made to use less memory, but both parts may be used frequently, so this is simpler
        protected virtual List<KeyValuePair<string, T>> PairList { get; set; } = new List<KeyValuePair<string, T>>();

        /// <summary>
        /// Ordered array of values in the collection.
        /// </summary>
        public virtual T[] ValueArr => ValueList.ToArray();
        public override IEnumerable<T> Values => ValueList;

        public virtual int IndexOf(T value) => ValueList.IndexOf(value);


        public virtual KeyValuePair<string, T> this[int index] => Get(index);

        public OrderedCollection() : base() { }
        public OrderedCollection(IKeyGenerator keyGenerator) : base(keyGenerator) { }

        protected override bool AddKeyedValue(string key, T value)
        {
            if (!base.AddKeyedValue(key, value))
                return false;

            PairList.Add(new KeyValuePair<string, T>(key, value));
            ValueList.Add(value);
            return true;
        }

        public override void Remove(string key)
        {
            var index = IndexOf(Collection[key]);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);

            base.Remove(key);
        }
        public override void Remove(T value)
        {
            var index = IndexOf(value);
            ValueList.RemoveAt(index);
            PairList.RemoveAt(index);

            base.Remove(value);
        }

        public virtual KeyValuePair<string, T> Get(int val) => PairList[val];

        public virtual void MoveValue(int newIndex, int currentIndex)
            => MoveValue(newIndex, ValueList[currentIndex]);
        public virtual void MoveValue(int newIndex, T value)
        {
            ValueList.Remove(value);
            ValueList.Insert(newIndex, value);
        }

        IEnumerator IEnumerable.GetEnumerator() => PairList.GetEnumerator();
        public override IEnumerator<KeyValuePair<string, T>> GetEnumerator() => PairList.GetEnumerator();

    }
}